using UnityEngine;

namespace _Source
{
    public class SpawnPrefabCommand : ICommand
    {
        private GameObject prefab;
        private GameObject spawnedObject;
        private Vector2 spawnPosition;

        public SpawnPrefabCommand(GameObject prefabToSpawn, Vector2 position)
        {
            prefab = prefabToSpawn;
            spawnPosition = position; // Сохраняем позицию при создании
        }

        public void Invoke(Vector2 position)
        {
            // Используем сохраненную позицию
            if (prefab != null)
            {
                spawnedObject = Object.Instantiate(prefab, spawnPosition, Quaternion.identity);
                Debug.Log($"Spawned prefab at {spawnPosition}");
            }
        }

        public void Undo()
        {
            if (spawnedObject != null)
            {
                Object.Destroy(spawnedObject);
                Debug.Log("Undo: Destroyed spawned prefab");
            }
        }
    }
}