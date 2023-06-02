using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerStaggerState : PlayerOnTrackState
{
    [SerializeField] private float _distance;
    
    public override void Enter()
    {
        base.Enter();
        if (Vector3.Distance(_tracker.PassedPoints[^1], _tracker.TargetPoints[0]) <
            _distance)
        {
            
            List<Vector3> path = CalculatePath(new List<Vector3>(), _distance, 0);
            transform.DOPath(path.ToArray(), _distance * _tracker.SecondsPerUnit, PathType.Linear).OnComplete(delegate (){ Debug.Log("SWITCH STATE BLYAT"); });

        }
        //If not then just move back along the point

    }

    private List<Vector3> CalculatePath(List<Vector3> points, float remainingDistance, int i)
    {
        i++;
        if (_tracker.PassedPoints.Count - 1 > i)
        {
            float d = Vector3.Distance(_tracker.PassedPoints[^i], points[^1]);
            if ( d < remainingDistance)
            {
                remainingDistance -= d;
                points.Add(_tracker.PassedPoints[^i]);
                return CalculatePath(points, remainingDistance, i);
            }
            else
            {
                Vector3 direction = (points[^1] - _tracker.PassedPoints[^i]).normalized;
                Vector3 endValue = points[^1] - (direction * remainingDistance);
                points.Add(endValue);
            }
        }

        return points;
    }
    
    protected override void MoveAlongPath()
    {
       
        
       // float percentageTravlled = (endValue - previousPoint).magnitude / (path[0] - previousPoint).magnitude;
    }

    private void PreviousPoint()
    {
        
    }
}


