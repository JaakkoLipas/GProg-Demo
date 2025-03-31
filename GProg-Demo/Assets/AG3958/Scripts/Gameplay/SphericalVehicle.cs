using UnityEngine;
using System.Collections;
using System;

namespace AG3958
{
    public class SphericalVehicle : Vehicle
    {
        [SerializeField] private Camera attachedCamera;

        protected override void Start()
        {
            OriginalMaxSpeed = MaxSpeed;
            rb = this.gameObject.GetComponent<Rigidbody>();
            rb.maxAngularVelocity = MaxSpeed;
            directionVector = attachedCamera.transform.forward;
        }

        protected override void FixedUpdate()
        {
            CurrentSpeed = rb.angularVelocity.magnitude;
            directionVector = attachedCamera.transform.forward;
        }

        public override IEnumerator ApplyPower()
        {
            while (Input.GetKey(KeyCode.W) || Input.GetAxis("Vertical") > 0.1f)
            {
                if (CurrentSpeed < MaxSpeed)
                {
                    if (Controls == IRacingVehicle.ControlMethod.Keyboard) rb.AddTorque(base.CalculatePower() * directionVector, ForceMode.VelocityChange);
                    else if (Controls == IRacingVehicle.ControlMethod.Controller) rb.AddTorque((base.CalculatePower() * Input.GetAxis("Vertical")) * directionVector, ForceMode.VelocityChange);
                }
                yield return new WaitForFixedUpdate();
            }
        }

        public override IEnumerator ApplyBrakes()
        {
            while (Input.GetKey(KeyCode.S) || Input.GetAxis("Vertical") < -0.1f)
            {
                if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.0f))
                {
                    if (Controls == IRacingVehicle.ControlMethod.Keyboard) rb.angularVelocity *= Math.Min((Weight / BrakingForce), 0.999f);
                    else if (Controls == IRacingVehicle.ControlMethod.Controller) rb.angularVelocity *= Math.Min((Weight / BrakingForce) * -Input.GetAxis("Vertical"), 0.999f);
                }
                yield return new WaitForFixedUpdate();
            }
        }

        public override IEnumerator ApplyBoost(float boostPower, float boostTime)
        {
            BoostActive = true;
            MaxSpeed += boostPower;
            rb.AddTorque((CurrentSpeed + boostPower) * directionVector, ForceMode.VelocityChange);
            yield return new WaitForSeconds(boostTime);
            StartCoroutine(DecelBoost());
        }

        public override IEnumerator DecelBoost()
        {
            BoostActive = false;
            MaxSpeed = OriginalMaxSpeed;
            while (CurrentSpeed > MaxSpeed * 1.01f)
            {
                if (BoostActive) break;
                rb.AddTorque(-0.2f * directionVector, ForceMode.VelocityChange);
                yield return new WaitForFixedUpdate();
            }
        }
    }
}