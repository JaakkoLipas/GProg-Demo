using System.Collections.Generic;
using UnityEngine;
using AG3958;

namespace AG3958
{
    public class StaircaseMover : MonoBehaviour
    {
        [Tooltip("Speed of the raise/lower")]
        [SerializeField] private int speed;

        [Tooltip("List of objects for the script to raise/lower from its containing surface")]
        [SerializeField] private List<Transform> objectsToMove;

        [Tooltip("If this is checked, the action is inverted (objects lower instead of raising and start from the end of the list instead of the start)")]
        [SerializeField] private bool invert;

        [Tooltip("If this is checked, the trigger plate is reset after the end of the action and the action is inverted")]
        [SerializeField] private bool repeatable;

        // Index of the current object being moved in the list of objects to move
        private int objectIndex;

        // Set the object to start from
        private void Awake()
        {
            if (invert) objectIndex = objectsToMove.Count;
            else objectIndex = 0;
        }

        // Trigger the plate and sink it into its containing surface.
        // Only the player and the player's projectiles can activate these plates.
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Player" || other.tag != "Projectile") return;
            
        }
    }

}