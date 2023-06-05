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
        Debug.Log("Hi");
        //base.Enter();
        if (Vector3.Distance(_tracker.PassedPoints[^1], _moveTarget.transform.position) <
            _distance)
        {
            
            List<Vector3> path = CalculatePath(new List<Vector3>() { _moveTarget.transform.position}, _distance, 0);
            _moveTarget.transform.DOPath(path.ToArray(), _duration, PathType.Linear)
                .OnComplete(delegate (){  OnComplete?.Invoke();}).SetEase(_curve);
            _tracker.MoveTimer = (path[^1] - _tracker.PassedPoints[^1]).magnitude / (_tracker.TargetPoints[0] - _tracker.PassedPoints[^1]).magnitude;
        }
        else
        {
            Vector3 direction = (_tracker.PassedPoints[^1]- _tracker.TargetPoints[0]).normalized;
            Vector3 endValue = (direction * _distance) - _moveTarget.transform.position;
            _moveTarget.transform.DOMove(endValue, _duration).SetEase(_curve)
                .OnComplete(delegate() { OnComplete?.Invoke(); });
        }
        //If not then just move back along the point

    }

    private List<Vector3> CalculatePath(List<Vector3> points, float remainingDistance, int i)
    {
        i++;
        if (_tracker.PassedPoints.Count - i >= 0)
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

        for (int j = 0; j < points.Count; j++)
        {
            if (!_tracker.TargetPoints.Contains(points[j]))
            {
                _tracker.TargetPoints.Insert(0, points[j]);
            }

            if (_tracker.PassedPoints.Contains(points[j]))
            {
                _tracker.PassedPoints.Remove(points[j]);
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


