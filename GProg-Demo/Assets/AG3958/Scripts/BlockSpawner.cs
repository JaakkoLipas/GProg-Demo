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

        [Tooltip("If set to true, the spawn origin will move to the position of the last spawned object when the final block is spawned")]
        public bool isExtending;

        // Spawn target vector
        private Vector3 spawnTarget;

        // Origin vector for spawning
        internal Vector3 spawnOrigin { get; private set; }

        // List of objects spawned by this script
        public List<GameObject> spawnedItems;

        // Initialize origin to transform position and target to origin
        private void Start()
        {
            spawnOrigin = transform.position;
            spawnTarget = SetSpawnPosition(0);
        }

        /// <summary>
        /// Spawns the desired amount of blocks in the given direction.
        /// </summary>
        private void SpawnBlocks()
        {
            for (int i = 1; i < spawnCount + 1; i++)
            {
                GameObject newBlock = new GameObject("Spawned Block " + i);
                newBlock.transform.SetParent(transform);
                newBlock.transform.position = spawnTarget;
                VisualizeBlocks(ref newBlock);

                newBlock.AddComponent<DestructibleBlock>();
                spawnedItems.Add(newBlock);
                spawnTarget = SetSpawnPosition(i);
            }

            Debug.Log(spawnedItems);
            // If the spawner is set to extend further instead of continuing to spawn, set the spawner's position to where the final block was spawned
            if (isExtending) spawnOrigin = spawnTarget;
            else spawnTarget = SetSpawnPosition(0); // Reset to the initial spawn point
        }

        /// <summary>
        /// Generates a primitive physics object of a chosen primitive type to physically represent a generated GameObject.
        /// </summary>
        /// <param name="obj">GameObject to give a visual and physical representation to.</param>
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
        public Vector3 SetSpawnPosition(int scale)
        {
            Vector3 newPos = spawnOrigin;
            float objectScale;
            if (scale != 0)
            {
                if (blockShape == PrimitiveType.Cube || blockShape == PrimitiveType.Cylinder) objectScale = 1.0f;
                else if (blockShape == PrimitiveType.Sphere) objectScale = 1.5f;
                else if (blockShape == PrimitiveType.Capsule) objectScale = 1.2f;
                else objectScale = 0.5f;
                objectScale = objectScale * scale;

                if (direction == Direction.Up) newPos = new Vector3(spawnOrigin.x, spawnOrigin.y + objectScale, spawnOrigin.z);
                else if (direction == Direction.Down) newPos = new Vector3(spawnOrigin.x, spawnOrigin.y - objectScale, spawnOrigin.z);
                else if (direction == Direction.Left) newPos = new Vector3(spawnOrigin.x - objectScale, spawnOrigin.y, spawnOrigin.z);
                else if (direction == Direction.Right) newPos = new Vector3(spawnOrigin.x + objectScale, spawnOrigin.y, spawnOrigin.z);
                else if (direction == Direction.Forward) newPos = new Vector3(spawnOrigin.x, spawnOrigin.y, spawnOrigin.z + objectScale);
                else if (direction == Direction.Back) newPos = new Vector3(spawnOrigin.x, spawnOrigin.y, spawnOrigin.z - objectScale);
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