using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace AG3958
{
    public class BoostPowerStrip : TrackObject
    {
        [Tooltip("How much should be added to the boost gauge per FixedUpdate tick. Set negative to turn into a hazard!")]
        [SerializeField] private float boostGaugePerTick;
        private List<IBoostable> boostablesOnStrip;

        private void Start()
        {
            this.TypeOfObject = "Boost Power Strip";
            this.CollisionTagList = new List<string>() { "Vehicle", "Player" };
            boostablesOnStrip = new List<IBoostable>();
        }

        private void FixedUpdate()
        {
            foreach (IBoostable vehicle in boostablesOnStrip)
            {
                if (vehicle.BoostGaugeLevel < vehicle.BoostGaugeMax) vehicle.BoostGaugeLevel += boostGaugePerTick;
                else vehicle.BoostGaugeLevel = vehicle.BoostGaugeMax;
            }
        }

        public override void OnTriggerEnter(Collider other)
        {
            foreach (string tag in CollisionTagList)
            {
                if (other.gameObject.CompareTag(tag))
                {
                    var v = other.gameObject.GetComponent<IBoostable>();
                    if (v != null)
                    {
                        boostablesOnStrip.Add(v);
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            foreach (string tag in CollisionTagList)
            {
                if (other.gameObject.CompareTag(tag))
                {
                    var v = other.gameObject.GetComponent<IBoostable>();
                    if (v != null)
                    {
                        boostablesOnStrip.Remove(v);
                    }
                }
            }
        }
    }
}