using UnityEngine;
using UnityEngine.Events;
using AG3958;

namespace AG3958
{
    public class CheckpointTrigger : MonoBehaviour
    {
        private UnityEvent l_VehicleEnter;

        private void Start()
        {
            if (l_VehicleEnter == null)
            {
                l_VehicleEnter = new UnityEvent();
            }

            l_VehicleEnter.AddListener(OnVehicleEnter);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Vehicle"))
            {
                l_VehicleEnter.Invoke();
            }
        }

        private void OnVehicleEnter()
        {
            GameObject thisObject = this.gameObject;
            FindFirstObjectByType<CheckpointSystem>().CheckpointTrigger(ref thisObject);
        }
    }

}