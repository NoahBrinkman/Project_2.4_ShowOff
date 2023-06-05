 using System.Collections.Generic;
 using UnityEngine;

 public class PlayerOnTrackState : PlayerState
 {
     [Header("Base")]
     [SerializeField] protected PathTracker _tracker;
     [SerializeField] protected Transform _moveTarget;
     
     
     protected bool _moving = false;
     protected bool _rotating = false;

     protected float totalMoveTime;
     protected float totalRotationTime;
     private Quaternion _rotationTarget;
     
     public override void Enter()
     {
         base.Enter();
         if (_tracker.TargetPoints.Count > 0)
         {
             if (_tracker.PassedPoints.Count > 0)
             {
                totalMoveTime = Vector3.Distance(_tracker.PassedPoints[^1], _tracker.TargetPoints[0]) * _tracker.SecondsPerUnit;
                if (_tracker.MoveTimer > 0)
                {
                    _tracker.MoveTimer = Mathf.Lerp(0, totalMoveTime, _tracker.MoveTimer);
                }
             }
             else
             {
                 totalMoveTime = Vector3.Distance(_moveTarget.transform.position, _tracker.TargetPoints[0]) * _tracker.SecondsPerUnit;
                 if (_tracker.MoveTimer > 0)
                 {
                     _tracker.MoveTimer = Mathf.Lerp(0, totalMoveTime, _tracker.MoveTimer);
                 }
             }
             NextRotatePoint();
         }
     }

     protected virtual void MoveAlongPath()
     {
         if (_moving)
         {
             _tracker.MoveTimer += Time.deltaTime;
             Vector3 newPos = Vector3.LerpUnclamped(_tracker.PassedPoints[_tracker.PassedPoints.Count-1], _tracker.TargetPoints[0], _tracker.MoveTimer / totalMoveTime);
             _moveTarget.transform.position = newPos;
             if (_tracker.MoveTimer >= totalMoveTime)
             {
                 NextMovePoint();
             }
         }
     }

     private void RotateAlongPath(){
         if (_rotating)
         {
             _tracker.RotationTimer += Time.deltaTime;
             _moveTarget.rotation = Quaternion.Slerp(_moveTarget.rotation, _rotationTarget,  _tracker.RotationTimer / totalRotationTime);
     
             if (_tracker.RotationTimer >= totalRotationTime)
             {
                 _rotating = false;
             }
         }
    }
     
     public override void Run()
     { 
         base.Run();
         MoveAlongPath();
         RotateAlongPath();
     }
    
     private void NextMovePoint()
     {
         _tracker.PassedPoints.Add(_tracker.TargetPoints[0]);
         _tracker.TargetPoints.RemoveAt(0);
         if (_tracker.TargetPoints.Count > 0)
         {
            // ResetRotation(path[0]);
             totalMoveTime = Vector3.Distance(_moveTarget.position, _tracker.TargetPoints[0]) * _tracker.SecondsPerUnit;
             _tracker.MoveTimer = 0;
             NextRotatePoint();
             
         }
         else
         {
             _moving = false;
         }

     }

     private void NextRotatePoint()
     {
         Vector3 relativePos =   _tracker.TargetPoints[0] -_moveTarget.position ;

         _rotationTarget = Quaternion.LookRotation(relativePos);
         float a = Quaternion.Angle( _rotationTarget, transform.rotation);
//         Debug.Log($"Angle: {a}");
         
         if (a <= _tracker.RotationMargin) return;
         
         totalRotationTime =  a * _tracker.SecondsPerDegree;
         _rotating = true;
         _tracker.RotationTimer = 0;
     }
     
     public void SetPath(List<Vector3> pPath)
     {

         for (int i = 0; i < pPath.Count; i++)
         {
             _tracker.TargetPoints.Add(pPath[i]);
         }
         totalMoveTime = Vector3.Distance(_moveTarget.transform.position,  _tracker.TargetPoints[0]) * _tracker.SecondsPerUnit;
         _tracker.PassedPoints.Add( _moveTarget.transform.position);
         _tracker.MoveTimer = 0;
         _moving = true;
     }
    
     public void AddToPath(Vector3 pPath)
     {
         if(!_tracker.TargetPoints.Contains(pPath))
             _tracker.TargetPoints.Add(pPath);
     }
     
     public void AddToPath(List<Vector3> pPath)
     {
         for (int i = 0; i < pPath.Count; i++)
         {
             if(!_tracker.TargetPoints.Contains(pPath[i]))
                 _tracker.TargetPoints.Add(pPath[i]);
         }
     }
     
 }            

