using System.Collections.Generic;
using UnityEngine;

namespace AG3958
{
    public abstract class TrackObject : MonoBehaviour
    {
        internal string TypeOfObject { get; set; }
        internal List<string> CollisionTagList { get; set; }

        public abstract void OnTriggerEnter(Collider other);
    } 
}