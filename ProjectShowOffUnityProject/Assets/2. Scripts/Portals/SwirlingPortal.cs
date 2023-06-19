using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class SwirlingPortal : MonoBehaviour
{
    [HideInInspector] public RoadGenerator ParentGenerator;
    [SerializeField] private SwirlingPortal _linkedPortal;
    [FormerlySerializedAs("_outwardDirection")] public Vector3 OutwardDirection;
    [FormerlySerializedAs("_teleportPosition")] public Vector3 TeleportPosition;

    [SerializeField] private bool _showVisualAids;
    public RoadGenerator targetBiome;
    private void OnTriggerEnter(Collider other)
    {
        //If we can get the component store it for now in temporary variable P
        if (other.GetComponent<PlayerMovement>() is PlayerMovement p )
        {
            if (p != null)
            {
                DOTween.Kill(p.transform.parent);
                p.Teleport(_linkedPortal.transform.position + _linkedPortal.TeleportPosition, _linkedPortal.OutwardDirection);
            }
        }
    }

    public void Initialize()
    {
      
        try
        {
            ParentGenerator.DeterminePortalDestination(this);
            targetBiome.TargettedByPortal = true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
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
