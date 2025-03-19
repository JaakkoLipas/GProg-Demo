using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG3958
{
    public sealed class BoosterPad : TrackObject
    {
        [SerializeField] private float boostPower;
        [SerializeField] private float boostTime;
        private bool boostOn;
        private float originalSpeed;

        private void Start()
        {
            this.TypeOfObject = "Booster";
            this.CollisionTagList = new List<string>();
            this.CollisionTagList.Add("Vehicle");
            boostOn = false;
        }

        public override void OnTriggerEnter(Collider other)
        {
            foreach (string tag in CollisionTagList)
            {
                if (other.gameObject.CompareTag(tag))
                {
                    if (other.TryGetComponent<Vehicle>(out Vehicle v))
                    {
                        StartCoroutine(v.ApplyBoost(boostPower));
                    }
                }
            }
        }
    }
}