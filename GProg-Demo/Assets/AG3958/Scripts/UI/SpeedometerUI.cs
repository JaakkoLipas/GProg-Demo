using TMPro;
using UnityEngine;
using System;

namespace AG3958
{
    public class SpeedometerUI : MonoBehaviour
    {
        private int speedValue;
        private GameObject playerObject;
        private Rigidbody playerRb;
        private float playerSpeed;
        [SerializeField] private TextMeshProUGUI text;

        private void Start()
        {
            playerObject = GameObject.FindWithTag("Player");
            if (!playerObject.TryGetComponent<IRacingVehicle>(out IRacingVehicle vehicle)) { this.gameObject.SetActive(false); return; }
            playerRb = playerObject.GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            playerSpeed = playerRb.linearVelocity.magnitude;
            speedValue = (int)Math.Round(playerSpeed * 3.6f, 1); // Convert m/s to km/h and round the display value
            text.text = speedValue.ToString();
        }
    }
}