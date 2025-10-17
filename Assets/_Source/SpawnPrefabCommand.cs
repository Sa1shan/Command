using UnityEngine;

namespace _Source
{
    public class SpawnPrefabCommand : ICommand
    {
        private GameObject _prefab;
        private GameObject _spawnedObject;
        private Vector2 _spawnPosition;

        public SpawnPrefabCommand(GameObject prefabToSpawn, Vector2 position)
        {
            _prefab = prefabToSpawn;
            _spawnPosition = position;
        }

        public void Invoke(Vector2 position)
        {
            if (_prefab != null)
            {
                _spawnedObject = Object.Instantiate(_prefab, _spawnPosition, Quaternion.identity);
                Debug.Log($"Spawned prefab at {_spawnPosition}");
            }
        }

        public void Undo()
        {
            if (_spawnedObject != null)
            {
                Object.Destroy(_spawnedObject);
                Debug.Log("Undo: Destroyed spawned prefab");
            }
        }
    }
}