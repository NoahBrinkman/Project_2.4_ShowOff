 using UnityEngine;

 public class PlayerOnTrackState : PlayerState
 {
     [Header("Base")]
     [SerializeField] protected PathTracker _tracker;
     [SerializeField] protected Transform _moveTarget;
     
     
     protected bool _moving = false;
     protected bool _rotating = false;

     protected float totalMoveTime;

     public override void Enter()
     {
         base.Enter();
         if (_tracker.TargetPoints.Count > 0)
         {
             if(_tracker.PassedPoints.Count > 0)
             totalMoveTime = Vector3.Distance(_tracker.PassedPoints[^1], _tracker.TargetPoints[0]) * _tracker.SecondsPerUnit;
             else totalMoveTime = Vector3.Distance(_moveTarget.transform.position, _tracker.TargetPoints[0]) * _tracker.SecondsPerUnit;
         }
     }

     protected virtual void MoveAlongPath()
     {
         if (_moving)
         {
             _tracker.MoveTimer += Time.deltaTime;
             Vector3 newPos = Vector3.LerpUnclamped(_tracker.PassedPoints[^1], _tracker.TargetPoints[0], _tracker.MoveTimer / totalMoveTime);
             _moveTarget.transform.position = newPos;
             if (_tracker.MoveTimer >= totalMoveTime)
             {
                 NextMovePoint();
             }
         }
     }
     
     public override void Run()
     { 
         base.Run();
        MoveAlongPath();
     }

     private void NextMovePoint()
     {
         _tracker.PassedPoints.Add(_tracker.TargetPoints[0]);
         _tracker.TargetPoints.RemoveAt(0);
         if (_tracker.TargetPoints.Count > 1)
         {
            // ResetRotation(path[0]);
             totalMoveTime = Vector3.Distance(transform.position, _tracker.TargetPoints[0]) * _tracker.SecondsPerUnit;
             _tracker.MoveTimer = 0;
         }
         else
         {
             _moving = false;
         }

     }
     
     
 }            

