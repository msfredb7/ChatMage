using UnityEngine;
using System.Collections;


public delegate void TriggerEvent(ColliderInfo other, ColliderListener listener);
public delegate void CollisionEvent(ColliderInfo other, Collision2D collision, ColliderListener listener);

//[RequireComponent(typeof(ColliderInfo))]
public abstract class ColliderListener : MonoBehaviour
{
    public ColliderInfo info;
    public abstract TriggerEvent OnTriggerEnter { get; set; }
    public abstract TriggerEvent OnTriggerExit { get; set; }
    public abstract CollisionEvent OnCollisionEnter { get; set; }
    public abstract CollisionEvent OnCollisionExit { get; set; }
}
