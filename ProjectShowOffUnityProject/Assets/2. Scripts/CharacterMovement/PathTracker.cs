using System;
using System.Collections.Generic;
using System.Linq;
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
    public bool SnapMovement = true;

    private void Awake()
    {
         ClearPoints();
    }

    public void UpdateSpeed()
    { 
        SecondsPerUnit = Mathf.Pow(SecondsPerUnit, speedMultiplier);
     //SecondsPerUnit *= .99f;
    }

    public void ClearPoints()
    {
        TargetPoints = new List<TargetPoint>();
        PassedPoints = new List<Vector3>();
        RotationTargets = new List<Quaternion>();
    }
    public void CleanUpTargets()
    {
        TargetPoints = TargetPoints.Distinct().ToList();
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

