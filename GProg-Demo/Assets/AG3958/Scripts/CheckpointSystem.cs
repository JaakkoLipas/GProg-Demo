using System;
using UnityEngine;
using AG3958;
using UnityEngine.SceneManagement;

namespace AG3958
{
    public class CheckpointSystem : MonoBehaviour
    {
        // ID of the current track as defined by the Scene Build Order
        private int trackID;

        // Current region index
        private int region;

        // Is the current lap valid? A lap is not counted if this is set to false!
        private bool isLapValid;

        // Current lap. Increase by 1 each time a valid lap is completed, and check against maximum lap count value
        private int lapCounter;

        // Maximum laps to be ran in the race governed by this checkpoint system. Editor parameter.
        [SerializeField] private int maximumLaps; // TODO: user-selectable lap counts by taking this value from a menu instead

        // TimeData instance for this instance of the checkpoint system
        private TimeData currentTimeData;

        // Time value when checkpoint system is initialized, to be set again when the finish line is crossed on a valid lap
        [HideInInspector] public float TimeOnStart { get; private set; }

        // Time value when a lap is completed. Reset at every lap completion; use the public getter to set database lap times
        [HideInInspector] public float LapTime { get; private set; }

        [Tooltip("Every track checkpoint in order.")]
        [SerializeField] private GameObject[] checkpointRegions;

        // Loads the current track's time data into the database handler (if not yet loaded) and initializes checkpoint system values
        private void Start()
        {
            trackID = SceneManager.GetActiveScene().buildIndex;
            if (!TimeDB.IsLoaded()) TimeDB.LoadDatabaseFromFile(trackID);

            // This is decremented by 1 to make Length and IndexOf the same at the final element
            region = checkpointRegions.Length - 1;
            isLapValid = false;
            lapCounter = 1;
            currentTimeData.Initialize(trackID);
            TimeOnStart = Time.time;
        }

        /// <summary>
        /// Proceeds the checkpoint system logic based on the GameObject triggering this method.
        /// If a valid lap is completed, the time value is logged into a TimeData instance.
        /// If upon completion of a valid lap the lap counter's set maximum value is reached, pass the TimeData instance to the endstate handler.
        /// If the GameObject is not found in the checkpoint system's assigned checkpoint array, return and consider the lap invalid.
        /// </summary>
        /// <param name="triggerRegion">Reference to the GameObject that triggers this method</param>
        public void TriggerCheckpoint(ref GameObject triggerRegion)
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
                    currentTimeData.PushLapTime(LapTime);
                    Debug.Log("+1 lap: " + TimeData.TimeToString(LapTime));
                    lapCounter++;
                    if (lapCounter >= maximumLaps)
                    {
                        TimeDB.SaveSingleToDB(currentTimeData);
                        TimeDB.SaveDatabaseToFile(trackID);
                        Debug.Log("Race complete");
                        this.gameObject.SetActive(false);
                        // TODO: endstate handler for race completion
                    }
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