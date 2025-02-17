using System;
using UnityEngine;

namespace AG3958
{
    public class CheckpointSystem : MonoBehaviour
    {
        // Current region index
        private int region;

        // Is the current lap valid? A lap is not counted if this is set to false!
        private bool isLapValid;

        [Tooltip("Every track checkpoint in order.")]
        [SerializeField] private GameObject[] checkpointRegions;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            // This is decremented by 1 to make Length and IndexOf the same at the final element
            region = checkpointRegions.Length - 1;
            isLapValid = false;
        }

        /// <summary>
        /// Proceeds the checkpoint system logic based on the GameObject triggering this method.
        /// If the GameObject is not found in the checkpoint system's assigned checkpoint array, return and consider the lap invalid.
        /// </summary>
        /// <param name="triggerRegion">Reference to the GameObject that triggers this method</param>
        public void CheckpointTrigger(ref GameObject triggerRegion)
        {
            int regionIndex = Array.IndexOf(checkpointRegions, triggerRegion);
            if (regionIndex == -1) // Error handler conditional logic: Array.IndexOf returns -1 if the element is not found
            {
                isLapValid = false;
                return;
            }
            if (region - regionIndex == checkpointRegions.Length - 1) // If this is true, a lap is completed
            {
                if (isLapValid) // Only log a completed lap if the lap has never been invalidated prior to crossing the finish line
                {
                    Debug.Log("+1 lap");
                }
                isLapValid = true; // Reset the lap to be valid when the finish line is crossed
            }
            else if (region - regionIndex != -1) // If this is true, the checkpoints were not progressed in order and the lap is invalidated
            {
                isLapValid = false;
            } 
            region = regionIndex;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            for (int i = 0; i < checkpointRegions.Length; i++)
            {
                int nextIndex = i + 1;
                if (nextIndex >= checkpointRegions.Length)
                {
                    nextIndex -= checkpointRegions.Length;
                }

                Gizmos.DrawLine(checkpointRegions[i].transform.position, checkpointRegions[nextIndex].transform.position);
                Gizmos.DrawSphere(checkpointRegions[i].transform.position, 0.1f);
            }
        }
    }

}