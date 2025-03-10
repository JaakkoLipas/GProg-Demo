using System.Collections.Generic;
using UnityEngine;

public abstract class TrackObject : MonoBehaviour
{
    public abstract string TypeOfObject { get; set; }
    public abstract List<string> CollisionTagList { get; set; }

    public abstract void OnTriggerEnter(Collider other);
}