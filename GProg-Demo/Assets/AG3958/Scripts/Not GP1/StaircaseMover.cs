using System.Collections.Generic;
using UnityEngine;
using AG3958;
using System;

namespace AG3958
{
    [RequireComponent(typeof(Collider))]
    public class StaircaseMover : MonoBehaviour
    {
        [Tooltip("Speed of the raise/lower")]
        [SerializeField] private float speed = 1.0f;

        [Tooltip("List of objects for the script to raise/lower from its containing surface")]
        [SerializeField] private Transform[] objectsToMove;

        [Tooltip("List of target points for the objects to move to, must be paired to the exact object")]
        [SerializeField] private Transform[] targetPoints;

        [Tooltip("If this is checked, the action is inverted (objects lower instead of raising and start from the end of the list instead of the start)")]
        [SerializeField] private bool invert;

        [Tooltip("If this is checked, the trigger plate is reset after the end of the action and be re-triggerable")]
        [SerializeField] private bool repeatable;

        [Tooltip("If this is checked, the direction of the movement is inverted compared to the initial state after the end of the action")]
        [SerializeField] private bool invertOnRepeat;

        // Position vector for where the triggering object is to move
        // private Vector3 triggerMove;

        // Trigger object's physical representation
        private GameObject triggerParent;

        // Position vector for where the object to move is to move
        // private Vector3 objectMove;

        // Object bounds of the object to move
        // private Bounds objectBounds;

        private void Awake()
        {
            triggerParent = this.transform.parent.gameObject;
        }

        // Trigger the plate and sink it into its containing surface.
        // Only the player and the player's projectiles can activate these plates.
        private void OnCollisionEnter(Collision collData)
        {
            if (objectsToMove.Length != targetPoints.Length) return; // Abort operation if the arrays are not equal length
            if (collData.gameObject.tag != "Player" || collData.gameObject.tag != "Projectile") return;
            this.gameObject.SetActive(false); // Disable trigger while the behavior is still triggering
            triggerParent.SetActive(false); // Disable the trigger plate itself to show the trigger is not active
            
            if (invert) // Invert the process order if the invert boolean is active
            {
                for (int i = objectsToMove.Length; i > 0; i--)
                {
                    MoveObjectToTarget(i);
                }
            }
            else
            {
                for (int i = 0; i < objectsToMove.Length; i++)
                {
                    MoveObjectToTarget(i);
                }
            }
            if (repeatable) // If the action is repeatable, reset the position, re-enable the trigger
            {
                triggerParent.SetActive(true);
                if (invertOnRepeat) invert = !invert; // Reverse the invert boolean if invertOnRepeat is on
                this.gameObject.SetActive(true);
            }
        }

        private void MoveObjectToTarget(int index)
        {
            Transform current = objectsToMove[index];
            Transform target = targetPoints[index];
            while (current.transform.position != target.transform.position)
            {
                float step = speed * Time.deltaTime;
                current.transform.position = Vector3.MoveTowards(current.transform.position, target.transform.position, step);
            }
        }
    }

}