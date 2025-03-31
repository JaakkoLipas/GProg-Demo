using System.Collections.Generic;
using UnityEngine;

namespace AG3958
{
    public sealed class BoosterPad : TrackObject
    {
        [SerializeField] private float boostPower;
        [SerializeField] private float boostTime;

        private void Start()
        {
            this.TypeOfObject = "Boost Pad";
            this.CollisionTagList = new List<string>() { "Vehicle", "Player" };
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
                        StartCoroutine(v.ApplyBoost(boostPower, boostTime));
                    }
                }
            }
        }
    }
}