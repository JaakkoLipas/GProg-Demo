using System;
using UnityEngine;
using AG3958;
using System.Collections;

namespace AG3958
{
    [RequireComponent(typeof(Rigidbody))]
    public class GearedVehicle : Vehicle
    {
        [SerializeField, Range(1,9)] private int gearCount = 1;
        public int CurrentGear { get; private set; }
        private float gearRatio;
        private (float speedLowerBound, float speedUpperBound) speedEnvelope;

        protected override void Start()
        {
            OriginalMaxSpeed = MaxSpeed;
            CurrentGear = 1;
            rb = this.gameObject.GetComponent<Rigidbody>();
            directionVector = rb.transform.forward;
            gearRatio = OriginalMaxSpeed / gearCount;
            SetSpeedEnvelope();
        }

        protected override void FixedUpdate()
        {
            CurrentSpeed = rb.linearVelocity.magnitude;
            directionVector = rb.transform.forward;
        }

        protected override void Update()
        {
            if (!AIControlled)
            {
                if (Input.GetKeyDown(KeyCode.W)) StartCoroutine(base.ApplyPower());
                if (Input.GetKeyDown(KeyCode.S)) StartCoroutine(base.ApplyBrakes());
                if (Input.GetKeyDown(KeyCode.Space) && BoostGaugeLevel >= BoostGaugeUse)
                {
                    BoostGaugeLevel -= BoostGaugeUse;
                    StartCoroutine(base.ApplyBoost(BoostSpeed, BoostTime));
                }
                if (Input.GetKeyDown(KeyCode.A)) StartCoroutine(base.ApplyRotationLeft());
                if (Input.GetKeyDown(KeyCode.D)) StartCoroutine(base.ApplyRotationRight());
                if (Input.GetKeyDown(KeyCode.R)) { InReverse = !InReverse; CurrentGear = 1; }
                if (Input.GetKeyDown(KeyCode.Q) && CurrentGear < gearCount)
                {
                    CurrentGear++;
                    SetSpeedEnvelope();
                }
                if (Input.GetKeyDown(KeyCode.E) && CurrentGear > 1)
                {
                    CurrentGear--;
                    SetSpeedEnvelope();
                }
            }
        }

        private void SetSpeedEnvelope()
        {
            speedEnvelope.speedLowerBound = gearRatio * (CurrentGear - 1);
            speedEnvelope.speedUpperBound = gearRatio * CurrentGear;
        }

        public override float CalculatePower()
        {
            float powerToWeightRatio = EnginePower / Weight;
            if (InReverse) powerToWeightRatio *= -1;
            
            float finalPower = powerToWeightRatio;
            if (CurrentSpeed < speedEnvelope.speedLowerBound) finalPower = powerToWeightRatio - Math.Max((speedEnvelope.speedLowerBound - CurrentSpeed), 0f);
            else if (CurrentSpeed > speedEnvelope.speedUpperBound) finalPower = powerToWeightRatio - Math.Max((CurrentSpeed - speedEnvelope.speedUpperBound), 0f);
            return Math.Max(finalPower, 0.05f);
        }
    }
}