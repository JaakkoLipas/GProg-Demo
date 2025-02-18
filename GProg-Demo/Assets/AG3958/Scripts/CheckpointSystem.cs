using System;
using UnityEngine;
using AG3958;

namespace AG3958
{
    public class CheckpointSystem : MonoBehaviour
    {
        // Current region index
        private int region;

        // Is the current lap valid? A lap is not counted if this is set to false!
        private bool isLapValid;

        // Time value when checkpoint system is initialized, to be set again when the finish line is crossed on a valid lap
        [HideInInspector] public float TimeOnStart { get; private set; }

        // Time value when a lap is completed. Reset at every lap completion; use the public getter to set database lap times
        [HideInInspector] public float LapTime { get; private set; }

        [Tooltip("Every track checkpoint in order.")]
        [SerializeField] private GameObject[] checkpointRegions;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            // This is decremented by 1 to make Length and IndexOf the same at the final element
            region = checkpointRegions.Length - 1;
            isLapValid = false;
            TimeOnStart = Time.time;
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
                    LapTime = Time.time - TimeOnStart;
                    Debug.Log("+1 lap: " + TimeSpan.FromSeconds(LapTime).ToString("mm':'ss'.'fff"));
                    TimeOnStart = Time.time;
                }
                isLapValid = true; // Reset the lap to be valid when the finish line is crossed
            }
            else if (region - regionIndex != -1) // If this is true, the checkpoints were not progressed in order
            {
                if (region - regionIndex == 1) // If this is true, checkpoints are being progressed in reverse
                {
                    Debug.Log("Wrong Way!");
                }
                else isLapValid = false; // Invalidate the lap if checkpoints are outright being skipped
            } 
            region = regionIndex;
            Debug.Log(region + " + " + isLapValid);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < checkpointRegions.Length; i++)
            {
                int nextIndex = i + 1;
                if (nextIndex >= checkpointRegions.Length)
                {
                    nextIndex -= checkpointRegions.Length;
                    Gizmos.color = Color.red;
                }

                Gizmos.DrawLine(checkpointRegions[i].transform.position, checkpointRegions[nextIndex].transform.position);
                if (nextIndex != 0) Gizmos.DrawSphere(checkpointRegions[nextIndex].transform.position, 0.1f);
                else Gizmos.DrawCube(checkpointRegions[nextIndex].transform.position, new Vector3(0.2f, 0.2f, 0.2f));
            }
        }
    }

}