using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace AG3958
{
    public class BoostPickup : TrackObject
    {
        [Tooltip("How much Boost Gauge collecting this pickup restores")]
        [SerializeField] private float boostGaugeValue;

        [Tooltip("How long in seconds until the pickup re-enables")]
        [SerializeField] private float respawnTime;
        private float originalValue;

        private void Start()
        {
            this.TypeOfObject = "Boost Pickup";
            this.CollisionTagList = new List<string>() { "Vehicle", "Player" };
            originalValue = boostGaugeValue;
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
                        if ((v.BoostGaugeLevel + boostGaugeValue) < v.BoostGaugeMax && (v.BoostGaugeLevel + boostGaugeValue) > 0) v.BoostGaugeLevel += boostGaugeValue;
                        else if ((v.BoostGaugeLevel + boostGaugeValue) < 0) v.BoostGaugeLevel = 0;
                        else v.BoostGaugeLevel = v.BoostGaugeMax;
                        StartCoroutine(CycleRespawn());
                    }
                }
            }
        }

        private IEnumerator CycleRespawn()
        {
            boostGaugeValue = 0;
            this.transform.GetChild(0).gameObject.SetActive(false);
            yield return new WaitForSeconds(respawnTime);
            boostGaugeValue = originalValue;
            this.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}