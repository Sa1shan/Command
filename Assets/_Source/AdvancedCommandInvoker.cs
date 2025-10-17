using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace _Source
{
    public class AdvancedCommandInvoker : MonoBehaviour
    {
        [Header("Commands Configuration")]
        [SerializeField] private GameObject prefabToSpawn;
        [SerializeField] private Transform characterToMove;

        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI queueInfoText;
        [SerializeField] private TextMeshProUGUI historyInfoText;

        [Header("Queue Settings")]
        private const int MaxQueueSize = 10;
        
        private Queue<SpawnPrefabCommand> _rightClickCommandQueue = new Queue<SpawnPrefabCommand>();
        
        private Stack<ICommand> _commandHistory = new Stack<ICommand>();

        private void Start()
        {
            UpdateUI();
        }

        private void Update()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = GetMouseWorldPosition();
                ExecuteLeftClickCommand(mousePosition);
            }
            
            if (Input.GetMouseButtonDown(1))
            {
                Vector2 mousePosition = GetMouseWorldPosition();
                AddRightClickCommandToQueue(mousePosition);
            }
            
            if (Input.GetMouseButtonDown(2))
            {
                UndoLastCommand();
            }
            
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                ExecuteRightClickQueue();
            }
        }

        private void ExecuteLeftClickCommand(Vector2 position)
        {
            if (characterToMove != null)
            {
                ICommand moveCommand = new MoveCharacterCommand(characterToMove, position);
                moveCommand.Invoke(position);
                
                AddToHistory(moveCommand);
            
                Debug.Log("Left-click command executed immediately");
                UpdateUI();
            }
        }

        private void AddRightClickCommandToQueue(Vector2 position)
        {
            if (prefabToSpawn != null)
            {
                SpawnPrefabCommand newSpawnCommand = new SpawnPrefabCommand(prefabToSpawn, position);
                
                _rightClickCommandQueue.Enqueue(newSpawnCommand);
            
                Debug.Log($"Right-click command added to queue at position {position}. Queue size: {_rightClickCommandQueue.Count}");
                UpdateUI();
            }
        }

        private void ExecuteRightClickQueue()
        {
            if (_rightClickCommandQueue.Count == 0)
            {
                Debug.Log("Right-click queue is empty");
                return;
            }

            Debug.Log($"Executing right-click queue with {_rightClickCommandQueue.Count} commands");
            
            while (_rightClickCommandQueue.Count > 0)
            {
                SpawnPrefabCommand command = _rightClickCommandQueue.Dequeue();
                command.Invoke(Vector2.zero);
                
                AddToHistory(command);
            }

            Debug.Log("Right-click queue execution completed");
            UpdateUI();
        }

        private void AddToHistory(ICommand command)
        {
            if (_commandHistory.Count >= MaxQueueSize)
            {
                ClearOldestCommand();
            }

            _commandHistory.Push(command);
            Debug.Log($"Command added to history. History size: {_commandHistory.Count}");
        }

        private void ClearOldestCommand()
        {
            if (_commandHistory.Count > 0)
            {
                Stack<ICommand> tempStack = new Stack<ICommand>();
            
                while (_commandHistory.Count > 1)
                {
                    tempStack.Push(_commandHistory.Pop());
                }
            
                ICommand oldestCommand = _commandHistory.Pop();
                Debug.Log($"Removed oldest command from history");
            
                while (tempStack.Count > 0)
                {
                    _commandHistory.Push(tempStack.Pop());
                }
            }
        }

        public void UndoLastCommand()
        {
            if (_commandHistory.Count > 0)
            {
                ICommand lastCommand = _commandHistory.Pop();
                lastCommand.Undo();
                Debug.Log($"Undo performed. History size: {_commandHistory.Count}");
                UpdateUI();
            }
            else
            {
                Debug.Log("No commands to undo");
            }
        }

        private Vector2 GetMouseWorldPosition()
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -Camera.main.transform.position.z;
            return Camera.main.ScreenToWorldPoint(mousePos);
        }

        private void UpdateUI()
        {
            if (queueInfoText != null)
            {
                queueInfoText.text = $"Очередь спавна: {_rightClickCommandQueue.Count}";
            }

            if (historyInfoText != null)
            {
                historyInfoText.text = $"История команд: {_commandHistory.Count}/{MaxQueueSize}";
            }
        }
    }
}