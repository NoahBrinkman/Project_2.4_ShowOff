using System.Collections.Generic;
using UnityEngine;

public class PathTracker : MonoBehaviour
{
    public float SecondsPerUnit;
    public float MoveTimer;
    
    public List<Vector3> TargetPoints { get; private set; }
    public List<Vector3> PassedPoints { get; private set; }
}
