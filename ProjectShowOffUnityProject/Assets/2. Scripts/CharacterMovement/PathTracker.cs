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
   
    public List<Vector3> TargetPoints { get; private set; }
    public List<Vector3> PassedPoints { get; private set; }

    private void Awake()
    {
        TargetPoints = new List<Vector3>();
        PassedPoints = new List<Vector3>();
    }
}
