using UnityEngine;

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
        region = checkpointRegions.Length;
        isLapValid = false;
    }

    // Call from the checkpoint trigger script
    public void CheckpointTrigger(int regionIndex)
    {
        if (region - regionIndex == -1) // If this is true, the checkpoints are being progressed in order
        {
            isLapValid = true;
        }
        else if (region - regionIndex == checkpointRegions.Length) // If this is true, a lap is completed
        {
            if (isLapValid)
            {
                Debug.Log("+1 lap");
            }
        }
        else isLapValid = false;
        region = regionIndex;
    }
}
