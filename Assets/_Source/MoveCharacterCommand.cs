using UnityEngine;

namespace _Source
{
    public class MoveCharacterCommand : ICommand
    {
        private Transform _characterTransform;
        private Vector2 _previousPosition;
        private Vector2 _targetPosition;

        public MoveCharacterCommand(Transform character, Vector2 newPosition)
        {
            _characterTransform = character;
            if (_characterTransform != null)
            {
                _previousPosition = _characterTransform.position;
                _targetPosition = newPosition;
            }
        }

        public void Invoke(Vector2 position)
        {
            if (_characterTransform != null)
            {
                _previousPosition = _characterTransform.position;
                _targetPosition = position;
                _characterTransform.position = position;
                Debug.Log($"Moved character from {_previousPosition} to {position}");
            }
        }

        public void Undo()
        {
            if (_characterTransform != null)
            {
                Vector2 temp = _characterTransform.position;
                _characterTransform.position = _previousPosition;
                _previousPosition = temp;
            
                Debug.Log($"Undo: Returned character to {_characterTransform.position}");
            }
        }
    }
}