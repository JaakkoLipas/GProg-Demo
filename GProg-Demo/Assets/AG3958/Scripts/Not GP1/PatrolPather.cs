using System.Collections.Generic;
using UnityEngine;
using AG3958;

namespace AG3958
{
    public class PatrolPather : MonoBehaviour
    {
        // Adjust the speed for the application.
        [Tooltip("Movement speed")]
        public float speed = 1.0f;

        // The target (cylinder) position.
        private Transform target;

        // Index of the current pathing target in the point list.
        private int pathingIndex;

        [Tooltip("List of pathing points (Empty Object Transforms) for the object to move between. Script runs from top to bottom.")]
        [SerializeField] private List<Transform> pathingPoints;

        private void Awake()
        {
            target = pathingPoints[0];
            pathingIndex = 0;
        }

        private void Update()
        {
            // Move our position a step closer to the target.
            var step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);

            // Check if the position of the object and its current target are approximately equal.
            if (Vector3.Distance(transform.position, target.position) < 0.001f)
            {
                // Proceed to the next target point
                UpdateTarget();
            }
        }

        private void UpdateTarget()
        {
            pathingIndex++;
            if (pathingIndex >= pathingPoints.Count) pathingIndex = 0;
            target = pathingPoints[pathingIndex];
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            for (int i = 0; i < pathingPoints.Count; i++)
            {
                int nextIndex = i + 1;
                if (nextIndex >= pathingPoints.Count)
                {
                    nextIndex -= pathingPoints.Count;
                }

                Gizmos.DrawLine(pathingPoints[i].position, pathingPoints[nextIndex].position);
                Gizmos.DrawSphere(pathingPoints[i].position, 0.1f);
            }
        }
    }
}