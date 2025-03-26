using TMPro;
using UnityEngine;
using System;

namespace AG3958
{
    public class SpeedometerUI : MonoBehaviour
    {
        private int speedValue;
        private IRacingVehicle playerVehicle;
        [SerializeField] private TextMeshProUGUI text;

        private void Start()
        {
            if (GameObject.FindWithTag("Player").TryGetComponent<IRacingVehicle>(out IRacingVehicle vehicle))
            {
                playerVehicle = vehicle;
            }
            if (playerVehicle == null) { this.gameObject.SetActive(false); return; }
        }

        private void FixedUpdate()
        {
            speedValue = (int)Math.Round(playerVehicle.CurrentSpeed * 3.6f, 1); // Convert m/s to km/h and round the display value
            text.text = speedValue.ToString();
        }
    }

}