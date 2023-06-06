 using System;
 using System.Collections.Generic;
 using UnityEngine;

 public class PlayerOnTrackState : PlayerState
 {
     [Header("Base")]
     [SerializeField] protected Transform _moveTarget;

     [SerializeField, Range(0, 1)] private float _inertia;
     protected bool _moving = false;
     protected bool _rotating = false;

     protected float totalMoveTime;
     protected float totalRotationTime;
     private Quaternion _rotationTarget;
     


     private void Start()
     {

     }

     public override void Enter()
     {
         base.Enter();
         
         if (StateMachine.PathTracker.TargetPoints.Count > 0)
         {
             if (StateMachine.PathTracker.PassedPoints.Count > 0)
             {
                totalMoveTime = Vector3.Distance(StateMachine.PathTracker.PassedPoints[^1], StateMachine.PathTracker.TargetPoints[0]) * StateMachine.PathTracker.SecondsPerUnit;
                if (StateMachine.PathTracker.MoveTimer > 0)
                {
                    StateMachine.PathTracker.MoveTimer = Mathf.Lerp(0, totalMoveTime, StateMachine.PathTracker.MoveTimer);
                }
             }
             else
             {
                 totalMoveTime = Vector3.Distance(_moveTarget.transform.position, StateMachine.PathTracker.TargetPoints[0]) * StateMachine.PathTracker.SecondsPerUnit;
                 if (StateMachine.PathTracker.MoveTimer > 0)
                 {
                     StateMachine.PathTracker.MoveTimer = Mathf.Lerp(0, totalMoveTime, StateMachine.PathTracker.MoveTimer);
                 }
             }
             NextRotatePoint();
         }
     }

     protected virtual void MoveAlongPath()
     {
         if (_moving)
         {
             StateMachine.PathTracker.MoveTimer += Time.deltaTime;
             Vector3 newPos = Vector3.SlerpUnclamped(StateMachine.PathTracker.PassedPoints[StateMachine.PathTracker.PassedPoints.Count-1], StateMachine.PathTracker.TargetPoints[0], StateMachine.PathTracker.MoveTimer / totalMoveTime);
             _moveTarget.transform.position = newPos;
             if (StateMachine.PathTracker.MoveTimer >= totalMoveTime)
             {
                 NextMovePoint();
             }
         }
     }

     private void RotateAlongPath(){
         if (_rotating)
         {
             StateMachine.PathTracker.RotationTimer += Time.deltaTime;
             Quaternion newRot = Quaternion.Lerp(_moveTarget.rotation, _rotationTarget,  StateMachine.PathTracker.RotationTimer / totalRotationTime);
             _moveTarget.rotation =   Quaternion.Euler(newRot.eulerAngles * _inertia + _moveTarget.rotation.eulerAngles *( 1.0f-_inertia));
             if (StateMachine.PathTracker.RotationTimer >= totalRotationTime)
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
         StateMachine.PathTracker.PassedPoints.Add(StateMachine.PathTracker.TargetPoints[0]);
         StateMachine.PathTracker.TargetPoints.RemoveAt(0);
         if (StateMachine.PathTracker.TargetPoints.Count > 0)
         {
            // ResetRotation(path[0]);
             totalMoveTime = Vector3.Distance(_moveTarget.position, StateMachine.PathTracker.TargetPoints[0]) * StateMachine.PathTracker.SecondsPerUnit;
             StateMachine.PathTracker.MoveTimer = 0;
             NextRotatePoint();
             
         }
         else
         {
             _moving = false;
         }

     }

     private void NextRotatePoint()
     {
         Vector3 relativePos = _moveTarget.position - StateMachine.PathTracker.TargetPoints[0];
         _rotationTarget = Quaternion.LookRotation(relativePos);
         
         float a = Quaternion.Angle(_moveTarget.rotation, _rotationTarget);
         if (a <= StateMachine.PathTracker.RotationMargin) return;
         
         totalRotationTime =  a * StateMachine.PathTracker.SecondsPerDegree;
         _rotating = true;
         StateMachine.PathTracker.RotationTimer = 0;
     }
     
     public void SetPath(List<Vector3> pPath)
     {

         for (int i = 0; i < pPath.Count; i++)
         {
             StateMachine.PathTracker.TargetPoints.Add(pPath[i]);
         }
         totalMoveTime = Vector3.Distance(_moveTarget.transform.position,  StateMachine.PathTracker.TargetPoints[0]) * StateMachine.PathTracker.SecondsPerUnit;
         StateMachine.PathTracker.PassedPoints.Add( _moveTarget.transform.position);
         StateMachine.PathTracker.MoveTimer = 0;
         _moving = true;
     }
    
     public void AddToPath(Vector3 pPath)
     {
         if (!StateMachine.PathTracker.TargetPoints.Contains(pPath))
         {
             StateMachine.PathTracker.TargetPoints.Add(pPath);
             
         }
     }
     
     public void AddToPath(List<Vector3> pPath)
     {
         for (int i = 0; i < pPath.Count; i++)
         {
             if (!StateMachine.PathTracker.TargetPoints.Contains(pPath[i]))
             {
                 StateMachine.PathTracker.TargetPoints.Add(pPath[i]);

             }
             
         }
     }

     
     public void AddNewRoad(RoadPoints roadPoints)
     {
         if (roadPoints.gameObject == StateMachine.CurrentRoad)
         {
             return;
         }

         StateMachine.CurrentRoad = roadPoints.gameObject;
         if (roadPoints.CurvePoints.Count > 0)
         {
             AddToPath(roadPoints.CurvePoints);
         }
         else
         {
             AddToPath(roadPoints.AssetStart);
             AddToPath(roadPoints.AssetEnd);
         }
         SetPath(new List<Vector3>());
     }
 }            

