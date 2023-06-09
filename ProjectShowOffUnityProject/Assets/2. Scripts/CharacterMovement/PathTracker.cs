using System;
using System.Collections.Generic;
using UnityEngine;

public class PathTracker : MonoBehaviour
{
    public float SecondsPerUnit;
   [HideInInspector] public float MoveTimer;

   [HideInInspector] public float RotationTimer;
   public float SecondsPerDegree;
   public float RotationMargin;
   public List<TargetPoint> TargetPoints { get; private set; }
    public List<Vector3> PassedPoints { get; private set; }
    public List<Quaternion> RotationTargets { get; private set; }

    private void Awake()
    {
        TargetPoints = new List<TargetPoint>();
        PassedPoints = new List<Vector3>();
        RotationTargets = new List<Quaternion>();
    }

}

[Serializable]
public class TargetPoint
{
    public Vector3 position;
    public bool includeInRotation;

    public TargetPoint(Vector3 pPosition, bool pincludeInRotation = true)
    {
        position = pPosition;
        includeInRotation = pincludeInRotation;
    }
    
}

