using UnityEngine;

namespace _Source
{
    public class MoveCharacterCommand : ICommand
    {
        private Transform _characterTransform;
        private Vector2 previousPosition;
        private Vector2 targetPosition;

        public MoveCharacterCommand(Transform character, Vector2 newPosition)
        {
            _characterTransform = character;
            if (_characterTransform != null)
            {
                previousPosition = _characterTransform.position;
                targetPosition = newPosition;
            }
        }

        public void Invoke(Vector2 position)
        {
            if (_characterTransform != null)
            {
                // Сохраняем предыдущую позицию перед перемещением
                previousPosition = _characterTransform.position;
                targetPosition = position;
                _characterTransform.position = position;
                Debug.Log($"Moved character from {previousPosition} to {position}");
            }
        }

        public void Undo()
        {
            if (_characterTransform != null)
            {
                // Меняем местами предыдущую и текущую позицию
                Vector2 temp = _characterTransform.position;
                _characterTransform.position = previousPosition;
                previousPosition = temp;
            
                Debug.Log($"Undo: Returned character to {_characterTransform.position}");
            }
        }
    }
}