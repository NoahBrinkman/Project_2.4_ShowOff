using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class EventCollider : MonoBehaviour
{
    public BoxCollider Collider { get; private set; }
    public UnityEvent<Collider> OnCollisionEnterEvent;
    public UnityEvent<Collider> OnCollisionExitEvent;
    public UnityEvent<Collider> OnCollisionStayEvent;


    private void Awake()
    {
        Collider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        
       OnCollisionEnterEvent?.Invoke(other);
    }
    private void OnTriggerExit(Collider other)
    {
       OnCollisionExitEvent?.Invoke(other);
    }

    private void OnTriggerStay(Collider collisionInfo)
    {
        OnCollisionStayEvent?.Invoke(collisionInfo);
    }
}
