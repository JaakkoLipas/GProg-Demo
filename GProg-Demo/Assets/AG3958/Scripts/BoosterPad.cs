using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG3958
{
    public class BoosterPad : TrackObject
    {
        public override string TypeOfObject { get; set; }
        public override List<string> CollisionTagList { get; set; }

        [SerializeField] private float boostMagnitude;
        [SerializeField] private float boostTime;
        private bool boostOn;
        private float originalSpeed;
        private PatrolPather patroller;

        private void Start()
        {
            TypeOfObject = "Booster";
            CollisionTagList = new List<string>();
            CollisionTagList.Add("Vehicle");
            boostOn = false;
        }

        private void FixedUpdate()
        {
            if (boostOn)
            {
                patroller.speed = originalSpeed * boostMagnitude;
                StartCoroutine(BoostTime());
            }
        }

        IEnumerator BoostTime()
        {
            yield return new WaitForSeconds(boostTime);
            patroller.speed = originalSpeed;
            boostOn = false;
        }

        public override void OnTriggerEnter(Collider other)
        {
            foreach (string tag in CollisionTagList)
            {
                if (other.gameObject.CompareTag(tag))
                {
                    if (other.gameObject.TryGetComponent<PatrolPather>(out PatrolPather pp))
                    {
                        originalSpeed = pp.speed;
                        patroller = pp;
                        boostOn = true;
                    }
                }
            }
        }
    }
}