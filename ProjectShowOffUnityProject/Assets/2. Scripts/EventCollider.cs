using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class EventCollider : MonoBehaviour
{
    public BoxCollider Collider { get; private set; }
    public UnityEvent<Collision> OnCollisionEnterEvent;
    public UnityEvent<Collision> OnCollisionExitEvent;
    public UnityEvent<Collision> OnCollisionStayEvent;


    private void Awake()
    {
        Collider = GetComponent<BoxCollider>();
    }

    public void OnCollisionEnter(Collision collision)
    {

       OnCollisionEnterEvent?.Invoke(collision);
    }

    private void OnCollisionExit(Collision other)
    {
       OnCollisionExitEvent?.Invoke(other);
    }

    private void OnCollisionStay(Collision collisionInfo)
    {
        OnCollisionStayEvent?.Invoke(collisionInfo);
    }
}
