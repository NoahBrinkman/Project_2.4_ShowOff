using System;
using System.Collections.Generic;
using UnityEngine;

public class PathTracker : MonoBehaviour
{
    public float SecondsPerUnit;
    [SerializeField, Tooltip("if you want the character to slow down make this below 0 speed up? above 0")] private float speedMultiplier;
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

    public void UpdateSpeed()
    {
        SecondsPerUnit *= speedMultiplier;
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

