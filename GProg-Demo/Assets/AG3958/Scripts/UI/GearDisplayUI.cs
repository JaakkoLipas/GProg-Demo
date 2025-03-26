using TMPro;
using UnityEngine;

namespace AG3958
{
    public class GearDisplayUI : MonoBehaviour
    {
        private int gearNum;
        private GearedVehicle playerVehicle;
        [SerializeField] private TextMeshProUGUI text;

        private void Start()
        {
            if (GameObject.FindWithTag("Player").TryGetComponent<GearedVehicle>(out GearedVehicle playerGV))
            {
                playerVehicle = playerGV;
            }
            if (playerVehicle == null) { this.gameObject.SetActive(false); return; }
        }

        private void Update()
        {
            gearNum = playerVehicle.CurrentGear;
            text.text = gearNum.ToString();
        }
    } 
}
