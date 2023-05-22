using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SwirlingPortal : MonoBehaviour
{
    [SerializeField] private SwirlingPortal _linkedPortal;
    [FormerlySerializedAs("_outwardDirection")] public Vector3 OutwardDirection;
    [FormerlySerializedAs("_teleportPosition")] public Vector3 TeleportPosition;

    [SerializeField] private bool _showVisualAids;
    private void OnTriggerEnter(Collider other)
    {
        //If we can get the component store it for now in temporary variable P
        if (other.GetComponent<PlayerMovement>() is PlayerMovement p )
        {
            if (p != null)
            {
                p.Teleport(_linkedPortal.transform.position + _linkedPortal.TeleportPosition, _linkedPortal.OutwardDirection);
   
            }
        }
    }


    private void OnDrawGizmos()
    {
        if (_showVisualAids)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position + TeleportPosition, Vector3.one);
            Gizmos.DrawRay(transform.position, OutwardDirection);
        }
    }
}
