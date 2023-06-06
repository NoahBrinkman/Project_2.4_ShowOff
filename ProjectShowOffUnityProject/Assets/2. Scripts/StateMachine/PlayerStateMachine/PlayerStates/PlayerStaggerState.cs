using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStaggerState : PlayerOnTrackState
{
    [SerializeField] private float _distance;
    [SerializeField] private float _duration = 1;
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private UnityEvent OnComplete;
    public override void Enter()
    {

        //base.Enter();
        if (Vector3.Distance(StateMachine.PathTracker.PassedPoints[^1], _moveTarget.transform.position) <
            _distance)
        {
            
            List<Vector3> path = CalculatePath(new List<Vector3>() { _moveTarget.transform.position}, _distance, 0);
            _moveTarget.transform.DOPath(path.ToArray(), _duration, PathType.Linear)
                .OnComplete(delegate (){  OnComplete?.Invoke();}).SetEase(_curve);
            StateMachine.PathTracker.MoveTimer = (path[^1] - StateMachine.PathTracker.PassedPoints[^1]).magnitude / (StateMachine.PathTracker.TargetPoints[0] - StateMachine.PathTracker.PassedPoints[^1]).magnitude;
        }
        else
        {
            Vector3 direction = (StateMachine.PathTracker.PassedPoints[^1]- StateMachine.PathTracker.TargetPoints[0]).normalized;
            Vector3 endValue = (direction * _distance) - _moveTarget.transform.position;
            _moveTarget.transform.DOMove(endValue, _duration).SetEase(_curve)
                .OnComplete(delegate() { OnComplete?.Invoke(); });
           // StateMachine.PathTracker.MoveTimer = (endValue - StateMachine.PathTracker.PassedPoints[^1]).magnitude / (StateMachine.PathTracker.TargetPoints[0] - StateMachine.PathTracker.PassedPoints[^1]).magnitude;
        }
        //If not then just move back along the point

    }

    private List<Vector3> CalculatePath(List<Vector3> points, float remainingDistance, int i)
    {
        i++;
        if (StateMachine.PathTracker.PassedPoints.Count - i >= 0)
        {
            float d = Vector3.Distance(StateMachine.PathTracker.PassedPoints[^i], points[^1]);
            if ( d < remainingDistance)
            {
                remainingDistance -= d;
                points.Add(StateMachine.PathTracker.PassedPoints[^i]);
                return CalculatePath(points, remainingDistance, i);
            }
            else
            {
                Vector3 direction = (points[^1] - StateMachine.PathTracker.PassedPoints[^i]).normalized;
                Vector3 endValue = points[^1] - (direction * remainingDistance);
                points.Add(endValue);
            }
        }

        for (int j = 0; j < points.Count; j++)
        {
            if (!StateMachine.PathTracker.TargetPoints.Contains(points[j]))
            {
                StateMachine.PathTracker.TargetPoints.Insert(0, points[j]);
            }

            if (StateMachine.PathTracker.PassedPoints.Contains(points[j]))
            {
                StateMachine.PathTracker.PassedPoints.Remove(points[j]);
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


