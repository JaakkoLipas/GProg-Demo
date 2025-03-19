using System;
using UnityEngine;
using AG3958;
using System.Collections;

namespace AG3958
{
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
        [field: SerializeField] public float BoostSpeed { get; set; }
        public float OriginalMaxSpeed { get; set; }
        [field: SerializeField] public float BoostTime { get; set; }
        [field: SerializeField] public float BoostGaugeMax { get; set; }
        [field: SerializeField] public float BoostGaugeUse { get; set; }
        [field: SerializeField] public float BoostGaugeLevel { get; set; }
        public bool BoostActive { get; set; } = false;

        private Rigidbody rb;
        private Vector3 directionVector;

        private void Start()
        {
            OriginalMaxSpeed = MaxSpeed;
            rb = GetComponent<Rigidbody>();
            directionVector = rb.transform.forward;
        }

        private void FixedUpdate()
        {
            CurrentSpeed = rb.linearVelocity.magnitude;
            directionVector = rb.transform.forward;
        }

        private void Update()
        {
            if (!AIControlled)
            {
                if (Input.GetKeyDown(KeyCode.W)) StartCoroutine(ApplyPower());
                if (Input.GetKeyDown(KeyCode.S)) StartCoroutine(ApplyBrakes());
                if (Input.GetKeyDown(KeyCode.Space) && BoostGaugeLevel >= BoostGaugeUse)
                {
                    BoostGaugeLevel -= BoostGaugeUse;
                    StartCoroutine(ApplyBoost(BoostSpeed));
                }
                if (Input.GetKeyDown(KeyCode.A)) StartCoroutine(ApplyRotationLeft());
                if (Input.GetKeyDown(KeyCode.D)) StartCoroutine(ApplyRotationRight());
                if (Input.GetKeyDown(KeyCode.R)) InReverse = !InReverse;
            }
        }

        public IEnumerator ApplyPower()
        {
            while (Input.GetKey(KeyCode.W))
            {
                if (CurrentSpeed < MaxSpeed)
                {
                    if (InReverse) rb.AddForce(-(EnginePower / Weight) * directionVector, ForceMode.VelocityChange);
                    else rb.AddForce((EnginePower / Weight) * directionVector, ForceMode.VelocityChange);
                }
                yield return new WaitForFixedUpdate();
            }
        }

        public IEnumerator ApplyBrakes()
        {
            while (Input.GetKey(KeyCode.S))
            {
                if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.0f))
                {
                    rb.linearVelocity *= Math.Min((Weight / BrakingForce), 0.999f);
                }
                yield return new WaitForFixedUpdate();
            }
        }

        public IEnumerator ApplyRotationLeft()
        {
            while (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(0, -MaxSteeringAngle, 0);
                yield return new WaitForFixedUpdate();
            }
        }

        public IEnumerator ApplyRotationRight()
        {
            while (Input.GetKey(KeyCode.D))
            {
                transform.Rotate(0, MaxSteeringAngle, 0);
                yield return new WaitForFixedUpdate();
            }
        }

        public IEnumerator ApplyBoost(float boostPower)
        {
            BoostActive = true;
            MaxSpeed += boostPower;
            rb.AddForce((CurrentSpeed + boostPower) * directionVector, ForceMode.VelocityChange);
            yield return new WaitForSeconds(BoostTime);
            StartCoroutine(DecelBoost());
        }

        public IEnumerator DecelBoost()
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