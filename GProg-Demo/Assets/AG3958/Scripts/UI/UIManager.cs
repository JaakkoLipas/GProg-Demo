using UnityEngine;

namespace AG3958
{
    public class UIManager : MonoBehaviour
    {
        private static GameObject playerObject;
        private static IRacingVehicle playerVehicleScript;
        private static IBoostable playerBoostable;

        void Start()
        {
            playerObject = GameObject.FindWithTag("Player");
            if (playerObject != null && playerObject.TryGetComponent<IRacingVehicle>(out IRacingVehicle playerVehicle))
            {
                playerVehicleScript = playerVehicle;
            }
            if (playerObject != null && playerObject.TryGetComponent<IBoostable>(out IBoostable playerBoost))
            {
                playerBoostable = playerBoost;
            }
        }

        public static GameObject GetPlayerObject()
        {
            return playerObject;
        }

        public static Vehicle GetPlayerVehicle()
        {
            return playerVehicleScript as Vehicle;
        }

        public static Vehicle GetPlayerBoostable()
        {
            return playerBoostable as Vehicle;
        }
    }
}