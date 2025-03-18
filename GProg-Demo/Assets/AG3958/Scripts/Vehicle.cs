using UnityEngine;
using AG3958;
using System.Collections;

namespace AG3958
{
    [RequireComponent(typeof(Rigidbody))]
    public class Vehicle : MonoBehaviour, IRacingVehicle, IBoostable
    {
        public float EnginePower { get; set; }
        public float Weight { get; set; }
        public float BrakingForce { get; set; }
        [HideInInspector] public float CurrentSpeed { get; set; }
        public float MaxSpeed { get; set; }
        public float MaxSteeringAngle { get; set; }
        [HideInInspector] public bool InReverse { get; set; }
        public bool AIControlled { get; set; } = false;
        public float BoostSpeed { get; set; }
        public float BoostTime { get; set; }
        public float BoostGaugeMax { get; set; }
        public float BoostGaugeUse { get; set; }
        public float BoostGaugeLevel { get; set; }
        [HideInInspector] public bool BoostActive { get; set; } = false;

        private Rigidbody rb;
        private Vector3 velocity;
        private Quaternion maxRotationLeft;
        private Quaternion maxRotationRight;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            velocity = Vector3.zero;
            maxRotationLeft = new Quaternion(-MaxSteeringAngle, Quaternion.identity.y, Quaternion.identity.z, Quaternion.identity.w);
            maxRotationRight = new Quaternion(MaxSteeringAngle, Quaternion.identity.y, Quaternion.identity.z, Quaternion.identity.w);
        }

        void FixedUpdate()
        {
            CurrentSpeed = rb.linearVelocity.magnitude;
        }

        void Update()
        {
            if (!AIControlled)
            {
                if (Input.GetKeyDown(KeyCode.W)) StartCoroutine(ApplyPower());
                if (Input.GetKeyDown(KeyCode.S)) StartCoroutine(ApplyBrakes());
                if (Input.GetKeyDown(KeyCode.Space) && BoostGaugeLevel >= BoostGaugeUse)
                {
                    BoostGaugeLevel -= BoostGaugeUse;
                    StartCoroutine(ApplyBoost());
                }
                if (Input.GetKeyDown(KeyCode.A)) StartCoroutine(ApplyRotationLeft());
                if (Input.GetKeyDown(KeyCode.D)) StartCoroutine(ApplyRotationRight());
            }
        }

        public IEnumerator ApplyPower()
        {
            while (Input.GetKey(KeyCode.W))
            {
                rb.linearVelocity = Mathf.Lerp(CurrentSpeed, MaxSpeed, (EnginePower / Weight)) * Vector3.forward;
                yield return new WaitForFixedUpdate();
            }
        }

        public IEnumerator ApplyBrakes()
        {
            while (Input.GetKey(KeyCode.S))
            {
                rb.linearVelocity = Mathf.Lerp(CurrentSpeed, 0, BrakingForce) * Vector3.forward;
                yield return new WaitForFixedUpdate();
            }
        }

        public IEnumerator ApplyRotationLeft()
        {
            while (Input.GetKey(KeyCode.A))
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, maxRotationLeft, 0.5f);
                yield return new WaitForFixedUpdate();
            }
        }

        public IEnumerator ApplyRotationRight()
        {
            while (Input.GetKey(KeyCode.D))
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, maxRotationRight, 0.5f);
                yield return new WaitForFixedUpdate();
            }
        }

        public IEnumerator ApplyBoost()
        {
            BoostActive = true;
            yield return new WaitForSeconds(BoostTime);
            StartCoroutine(DecelBoost());
        }

        public IEnumerator DecelBoost()
        {
            BoostActive = false;
            while (CurrentSpeed > MaxSpeed * 1.01f)
            {
                if (BoostActive) break;
                CurrentSpeed = Mathf.Lerp(CurrentSpeed, MaxSpeed, 0.02f);
                yield return new WaitForFixedUpdate();
            }
        }
    }
}