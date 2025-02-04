using System.Collections.Generic;
using UnityEngine;
using AG3958;

namespace AG3958
{
    public class BlockSpawner : MonoBehaviour
    {
        private enum Direction
        {
            Up,
            Down,
            Left,
            Right,
            Forward,
            Back
        }
        [Tooltip("Direction to spawn blocks in")]
        [SerializeField] private Direction direction;

        [Tooltip("Primitive shape of the blocks to be spawned")]
        [SerializeField] private PrimitiveType blockShape;

        [Tooltip("Number of blocks to spawn per spawn")]
        public int spawnCount = 1;

        // Spawn target vector
        private Vector3 spawnTarget;

        // List of objects spawned by this script
        public List<GameObject> spawnedItems;
        private void Start()
        {
            spawnTarget = SetSpawnPosition(0);
        }

        private void SpawnBlocks()
        {
            for (int i = 1; i < spawnCount + 1; i++)
            {
                GameObject newBlock = new GameObject("Spawned Block " + i);
                newBlock.transform.parent = transform;
                newBlock.transform.position = spawnTarget;
                VisualizeBlocks(ref newBlock);

                newBlock.AddComponent<DestructibleBlock>();
                spawnedItems.Add(newBlock);
                spawnTarget = SetSpawnPosition(i);
            }
            Debug.Log(spawnedItems);
        }

        private void VisualizeBlocks(ref GameObject obj)
        {
            GameObject visualizer = GameObject.CreatePrimitive(blockShape);
            visualizer.transform.SetParent(obj.transform);
            visualizer.transform.position = obj.transform.position;
        }

        /// <summary>
        /// Sets spawn vector to place new block onto. Call with 0 to reset back to origin.
        /// </summary>
        /// <param name="scale">Integer scale to increase spawn target vector by for each block.</param>
        /// <returns>Spawn target vector.</returns>
        private Vector3 SetSpawnPosition(int scale)
        {
            Vector3 newPos = transform.position;
            if (scale != 0)
            {
                if (direction == Direction.Up) newPos = new Vector3(spawnTarget.x, spawnTarget.y + scale, spawnTarget.z);
                else if (direction == Direction.Down) newPos = new Vector3(spawnTarget.x, spawnTarget.y - scale, spawnTarget.z);
                else if (direction == Direction.Left) newPos = new Vector3(spawnTarget.x - scale, spawnTarget.y, spawnTarget.z);
                else if (direction == Direction.Right) newPos = new Vector3(spawnTarget.x + scale, spawnTarget.y, spawnTarget.z);
                else if (direction == Direction.Forward) newPos = new Vector3(spawnTarget.x, spawnTarget.y, spawnTarget.z + scale);
                else if (direction == Direction.Back) newPos = new Vector3(spawnTarget.x, spawnTarget.y, spawnTarget.z - scale);
            }
            return newPos;
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T)) SpawnBlocks();
        }
    }

}