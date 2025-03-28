using System;
using UnityEngine;
using AG3958;
using System.Collections;

namespace AG3958
{
    /// <summary>
    /// Base class for controllable vehicles. As Vehicle's methods implementing IRacingVehicle and IBoostable
    /// use AddForce and Transform.forward, it is unsuitable for vehicles using spheres or wheels as the contact body.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class Vehicle : MonoBehaviour, IRacingVehicle, IBoostable
    {
        [field: SerializeField] public float EnginePower { get; set; }
        [field: SerializeField] public float Weight { get; set; }
        [field: SerializeField] public float BrakingForce { get; set; }
        public float CurrentSpeed { get; set; }
        [field: SerializeField] public float MaxSpeed { get; set; }
        [field: SerializeField] public float MaxSteeringAngle { get; set; }
        public bool InReverse { get; set; }
        [field: SerializeField] public bool AIControlled { get; set; } = false;
        [field: SerializeField] public IRacingVehicle.ControlMethod Controls { get; set; }
        [field: SerializeField] public float BoostSpeed { get; set; }
        public float OriginalMaxSpeed { get; set; }
        [field: SerializeField] public float BoostTime { get; set; }
        [field: SerializeField] public float BoostGaugeMax { get; set; }
        [field: SerializeField] public float BoostGaugeUse { get; set; }
        [field: SerializeField] public float BoostGaugeLevel { get; set; }
        public bool BoostActive { get; set; } = false;

        protected Rigidbody rb;
        protected Vector3 directionVector;

        protected virtual void Start()
        {
            OriginalMaxSpeed = MaxSpeed;
            rb = this.gameObject.GetComponent<Rigidbody>();
            directionVector = rb.transform.forward;
        }

        protected virtual void FixedUpdate()
        {
            CurrentSpeed = rb.linearVelocity.magnitude;
            directionVector = rb.transform.forward;
        }

        protected virtual void Update()
        {
            if (!AIControlled)
            {
                if (Controls == IRacingVehicle.ControlMethod.Keyboard)
                {
                    if (Input.GetKeyDown(KeyCode.W)) StartCoroutine(ApplyPower());
                    if (Input.GetKeyDown(KeyCode.S)) StartCoroutine(ApplyBrakes());
                    if (Input.GetKeyDown(KeyCode.Space) && BoostGaugeLevel >= BoostGaugeUse)
                    {
                        BoostGaugeLevel -= BoostGaugeUse;
                        StartCoroutine(ApplyBoost(BoostSpeed, BoostTime));
                    }
                    if (Input.GetKeyDown(KeyCode.A)) StartCoroutine(ApplyRotationLeft());
                    if (Input.GetKeyDown(KeyCode.D)) StartCoroutine(ApplyRotationRight());
                    if (Input.GetKeyDown(KeyCode.R)) InReverse = !InReverse;
                }
                else if (Controls == IRacingVehicle.ControlMethod.Controller)
                {

                }
            }
        }

        public virtual float CalculatePower()
        {
            float powerToWeightRatio = EnginePower / Weight;
            if (InReverse) powerToWeightRatio *= -1;
            return powerToWeightRatio;
        }

        public virtual IEnumerator ApplyPower()
        {
            while (Input.GetKey(KeyCode.W) || Input.GetAxis("Vertical") > 0.1f)
            {
                if (CurrentSpeed < MaxSpeed)
                {
                    if (Controls == IRacingVehicle.ControlMethod.Keyboard) rb.AddForce(CalculatePower() * directionVector, ForceMode.VelocityChange);
                    else if (Controls == IRacingVehicle.ControlMethod.Controller) rb.AddForce((CalculatePower() * Input.GetAxis("Vertical")) * directionVector, ForceMode.VelocityChange);
                }
                yield return new WaitForFixedUpdate();
            }
        }

        public virtual IEnumerator ApplyBrakes()
        {
            while (Input.GetKey(KeyCode.S) || Input.GetAxis("Vertical") < -0.1f)
            {
                if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.0f))
                {
                    if (Controls == IRacingVehicle.ControlMethod.Keyboard) rb.linearVelocity *= Math.Min((Weight / BrakingForce), 0.999f);
                    else if (Controls == IRacingVehicle.ControlMethod.Controller) rb.linearVelocity *= Math.Min((Weight / BrakingForce) * -Input.GetAxis("Vertical"), 0.999f);
                }
                yield return new WaitForFixedUpdate();
            }
        }

        public virtual IEnumerator ApplyRotationLeft()
        {
            while (Input.GetKey(KeyCode.A) || Input.GetAxis("Horizontal") < -0.1f)
            {
                if (Controls == IRacingVehicle.ControlMethod.Keyboard) transform.Rotate(0, -MaxSteeringAngle, 0);
                else if (Controls == IRacingVehicle.ControlMethod.Controller) transform.Rotate(0, -MaxSteeringAngle * -Input.GetAxis("Horizontal"), 0);
                yield return new WaitForFixedUpdate();
            }
        }

        public virtual IEnumerator ApplyRotationRight()
        {
            while (Input.GetKey(KeyCode.D) || Input.GetAxis("Horizontal") > 0.1f)
            {
                if (Controls == IRacingVehicle.ControlMethod.Keyboard) transform.Rotate(0, MaxSteeringAngle, 0);
                else if (Controls == IRacingVehicle.ControlMethod.Controller) transform.Rotate(0, MaxSteeringAngle * Input.GetAxis("Horizontal"), 0);
                yield return new WaitForFixedUpdate();
            }
        }

        public virtual IEnumerator ApplyBoost(float boostPower, float boostTime)
        {
            BoostActive = true;
            MaxSpeed += boostPower;
            rb.AddForce((CurrentSpeed + boostPower) * directionVector, ForceMode.VelocityChange);
            yield return new WaitForSeconds(boostTime);
            StartCoroutine(DecelBoost());
        }

        public virtual IEnumerator DecelBoost()
        {
            BoostActive = false;
            MaxSpeed = OriginalMaxSpeed;
            while (CurrentSpeed > MaxSpeed * 1.01f)
            {
                if (BoostActive) break;
                rb.AddForce(-0.2f * directionVector, ForceMode.VelocityChange);
                yield return new WaitForFixedUpdate();
            }
        }
    }
}