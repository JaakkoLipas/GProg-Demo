using UnityEngine;
using UnityEngine.UI;

namespace AG3958
{
    public class BoostGaugeUI : MonoBehaviour
    {
        private float gaugeMax;
        private float gaugeLevel;
        private IBoostable playerVehicle;
        [SerializeField] private Image gaugeMask;

        private void Start()
        {
            if (GameObject.FindWithTag("Player").TryGetComponent<IBoostable>(out IBoostable vehicle))
            {
                playerVehicle = vehicle;
            }
            if (playerVehicle == null) { this.gameObject.SetActive(false); return; }
            gaugeMax = playerVehicle.BoostGaugeMax;
        }

        private void FixedUpdate()
        {
            gaugeLevel = playerVehicle.BoostGaugeLevel;
            gaugeMask.fillAmount = gaugeLevel / gaugeMax;
        }
    }
}