using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG3958
{
    public sealed class BoosterPad : TrackObject
    {
        [SerializeField] private float boostMagnitude;
        [SerializeField] private float boostTime;
        private bool boostOn;
        private float originalSpeed;
        private PatrolPather patroller;
        private Rigidbody rb;

        private void Start()
        {
            this.TypeOfObject = "Booster";
            this.CollisionTagList = new List<string>();
            this.CollisionTagList.Add("Vehicle");
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

        private IEnumerator BoostTime()
        {
            yield return new WaitForSeconds(boostTime);
            boostOn = false;
            StartCoroutine(DecelGradual());
        }

        private IEnumerator DecelGradual()
        {
            while (patroller.speed > originalSpeed * 1.01f)
            {
                if (boostOn) break;
                patroller.speed = Mathf.Lerp(patroller.speed, originalSpeed, 0.02f);
                yield return new WaitForSeconds(0.1f);
            }
            patroller.speed = originalSpeed;
        }

        public override void OnTriggerEnter(Collider other)
        {
            foreach (string tag in CollisionTagList)
            {
                if (other.gameObject.CompareTag(tag))
                {
                    if (other.TryGetComponent<PatrolPather>(out PatrolPather pp))
                    {
                        patroller = pp;
                        originalSpeed = patroller.speed;
                        boostOn = true;
                    }
                }
            }
        }
    }
}