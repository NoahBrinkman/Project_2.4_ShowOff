 using System;
 using System.Collections.Generic;
 using DG.Tweening;
 using Unity.Mathematics;
 using Unity.VisualScripting;
 using UnityEngine;

 public class PlayerOnTrackState : PlayerState
 {
     [Header("Base")]
     [SerializeField] protected Transform _moveTarget;

     [SerializeField, Range(0, 5)] private float _smoothing;
     protected bool _moving = false;
     protected bool _rotating = false;

     protected float totalMoveTime;
     protected float totalRotationTime;
     private Quaternion _rotationTarget;
     


     private void Start()
     {

     }
    
     /// <summary>
     /// When entering this state, check if there is left over targets or passed points, if so snap the positino to hwere the player is supposed to be
     /// this is for staggering
     /// if there arent an passed points add where the player has started and start moving to the new target
     /// Set the rotationTarget
     /// </summary>
     public override void Enter()
     {
         base.Enter();
         
         if (StateMachine.PathTracker.TargetPoints.Count > 0)
         {
             if (StateMachine.PathTracker.PassedPoints.Count > 0)
             {
                totalMoveTime = Vector3.Distance(StateMachine.PathTracker.PassedPoints[^1], StateMachine.PathTracker.TargetPoints[0].position) * StateMachine.PathTracker.SecondsPerUnit;
                if (StateMachine.PathTracker.MoveTimer > 0)
                {
                    StateMachine.PathTracker.MoveTimer = Mathf.Lerp(0, totalMoveTime, StateMachine.PathTracker.MoveTimer);
                }
             }
             else
             {
                 StateMachine.PathTracker.PassedPoints.Add(_moveTarget.transform.position);
                 totalMoveTime = Vector3.Distance(_moveTarget.transform.position, StateMachine.PathTracker.TargetPoints[0].position) * StateMachine.PathTracker.SecondsPerUnit;
                 if (StateMachine.PathTracker.MoveTimer > 0)
                 {
                     StateMachine.PathTracker.MoveTimer = Mathf.Lerp(0, totalMoveTime, StateMachine.PathTracker.MoveTimer);
                 }
             }
             //NextRotatePoint();
         }
     }
     
     /// <summary>
     /// Lerp the player along the path
     /// </summary>
     protected virtual void MoveAlongPath()
     {
         if (_moving)
         {
             StateMachine.PathTracker.MoveTimer += Time.deltaTime;
             Vector3 newPos = Vector3.Lerp(StateMachine.PathTracker.PassedPoints[StateMachine.PathTracker.PassedPoints.Count-1], StateMachine.PathTracker.TargetPoints[0].position, StateMachine.PathTracker.MoveTimer / totalMoveTime);
             _moveTarget.transform.position = newPos;
             if ((StateMachine.PathTracker.TargetPoints[0].position - _moveTarget.transform.position).magnitude < .008f)
             {
                 //NextMovePoint();
             }
             if (StateMachine.PathTracker.MoveTimer >= totalMoveTime)
             {
                 NextMovePoint();
             }
         }
     }
    
     /// <summary>
     /// Rotate the player along the path
     /// </summary>
     /// ADDITIONAL NOTE:something might be going wrong here
     private void RotateAlongPath(){
         if (_rotating)
         {
            
             StateMachine.PathTracker.RotationTimer += Time.deltaTime;
             Quaternion newRot = Quaternion.Slerp(_moveTarget.rotation, _rotationTarget,  _smoothing * Time.deltaTime);
             
             
             _moveTarget.rotation = newRot;

            /*Vector3 newRot = new Vector3();
            newRot.x = _rotationTarget.eulerAngles.x;
            newRot.y = _rotationTarget.eulerAngles.y;
            newRot.z = _rotationTarget.eulerAngles.z;*/
           // _moveTarget.eulerAngles = newRot;
             //_moveTarget.rotation = Quaternion.Euler(newRot.eulerAngles * _inertia + _moveTarget.rotation.eulerAngles *( 1.0f-_inertia));
             if (Quaternion.Angle(newRot, _rotationTarget) < 0.08f)
             {
                 Quaternion newRota = Quaternion.Slerp(_moveTarget.rotation, _rotationTarget,  1);
             
             
                 _moveTarget.rotation = newRota;
                 NextRotatePoint();
             }
         }
     }
     /// <summary>
     /// Update()
     /// </summary>
     public override void Run()
     { 
         base.Run();
         if (Input.GetKeyDown(KeyCode.J))
         {
             _smoothing -= .1f;
         }else if (Input.GetKeyDown(KeyCode.K))
         {
             _smoothing += .1f;
         }
         MoveAlongPath();
         //Debug.Log(StateMachine.PathTracker.TargetPoints.Count);
         RotateAlongPath();
     }
    
     /// <summary>
     /// Remove the first index from target points and add it to passed points
     /// if there are more points to move to
     /// set the time and start moving set the next rotate point
     /// </summary>
     private void NextMovePoint()
     {
         if (totalMoveTime > StateMachine.PathTracker.MoveTimer)
         {
             Debug.LogError("The fuck is happening here");
         }
         StateMachine.PathTracker.PassedPoints.Add(StateMachine.PathTracker.TargetPoints[0].position);
         StateMachine.PathTracker.TargetPoints.RemoveAt(0);
         
         if (StateMachine.PathTracker.TargetPoints.Count > 0)
         {
             totalMoveTime = Vector3.Distance(_moveTarget.position, StateMachine.PathTracker.TargetPoints[0].position) * StateMachine.PathTracker.SecondsPerUnit;
             StateMachine.PathTracker.MoveTimer = 0;
             NextRotatePoint();
             // ResetRotation(path[0]);

         }
         else
         {
             _moving = false;
         }

     }
     /// <summary>
     /// Set the next rotational point between the player and the new target
     /// decide if the angle is big enough to actually rotate the player (less jitter)
     /// and set the time.
     /// </summary>
     /// ADDITIONAL NOTE: Probably the cause of madness
     private void NextRotatePoint()
     {
         if(StateMachine.PathTracker.TargetPoints.Count <= 0) return;
         if(!StateMachine.PathTracker.TargetPoints[0].includeInRotation) return;
        
         Vector3 relativePos =  _moveTarget.position - StateMachine.PathTracker.TargetPoints[0].position;
         relativePos.y = 0;
         if(Vector3.Dot(relativePos, _moveTarget.forward) < 0f) return;
//         Debug.Log("CURRENT POINT: " + StateMachine.PathTracker.PassedPoints[^1] +"TARGET POINT: " + StateMachine.PathTracker.TargetPoints[0] + "Relative Pos: " + relativePos);
         _rotationTarget = Quaternion.LookRotation(relativePos.normalized);
         
         float a = Quaternion.Angle( _rotationTarget, _moveTarget.rotation);
//         Debug.LogError(a);
  
        // if (a <= StateMachine.PathTracker.RotationMargin) return;
         
         totalRotationTime =  a * StateMachine.PathTracker.SecondsPerDegree;
         _rotating = true;
         StateMachine.PathTracker.RotationTimer = 0;
     }
     
     public void SetPath(List<TargetPoint> pPath)
     {

         for (int i = 0; i < pPath.Count; i++)
         {
             StateMachine.PathTracker.TargetPoints.Add(pPath[i]);
         }
         totalMoveTime = Vector3.Distance(_moveTarget.transform.position,  StateMachine.PathTracker.TargetPoints[0].position) * StateMachine.PathTracker.SecondsPerUnit;
         StateMachine.PathTracker.PassedPoints.Add( _moveTarget.transform.position);
         StateMachine.PathTracker.MoveTimer = 0;
         _moving = true;
         //AddRotations(pPath);
     }
    
     public void AddToPath(TargetPoint pPath)
     {
         if (!StateMachine.PathTracker.TargetPoints.Contains(pPath))
         {
             StateMachine.PathTracker.TargetPoints.Add(pPath);
             //  AddRotations(pPath, _moveTarget.position);
         }
     }
     
     public void AddToPath(List<TargetPoint> pPath)
     {
         for (int i = 0; i < pPath.Count; i++)
         {
             if (!StateMachine.PathTracker.TargetPoints.Contains(pPath[i]))
             {
                 StateMachine.PathTracker.TargetPoints.Add(pPath[i]);
             }
             
         }

         //AddRotations(pPath);
     }


     /*
     protected void AddRotations(List<Vector3> pPoints)
      {
          var sequence = DOTween.Sequence();
          
          /*Vector3 relativePos = _moveTarget.position - StateMachine.PathTracker.TargetPoints[0];
          _rotationTarget = Quaternion.LookRotation(relativePos);
          
          float a = Quaternion.Angle( _rotationTarget, _moveTarget.rotation);
          if (a <= StateMachine.PathTracker.RotationMargin) return;
          
          totalRotationTime =  a * StateMachine.PathTracker.SecondsPerDegree;
          _rotating = true;
          StateMachine.PathTracker.RotationTimer = 0;#1#
          List<Quaternion> rotations = new List<Quaternion>(); 
          for (int i = 1; i < pPoints.Count; i++)
          {
              Vector3 relativePos = pPoints[i - 1] - pPoints[i];
              Quaternion endRotation = Quaternion.LookRotation(relativePos);
              float a;
              if (rotations.Count > 0)
              {
                  a = Quaternion.Angle(rotations[^1], endRotation);
              }
              else
              {
                  a = Quaternion.Angle(_moveTarget.rotation, endRotation);
              }
 
              if (a >= StateMachine.PathTracker.RotationMargin)
              {
                 rotations.Add(endRotation);
                 sequence.Append(_moveTarget.DORotateQuaternion(endRotation,
                     a * StateMachine.PathTracker.SecondsPerDegree).SetEase(Ease.Linear));
              }
          }
 
          sequence.Play();
      }
      
      protected void AddRotations(Vector3 pPoint, Vector3 currentPosition)
      {
          //var sequence = DOTween.Sequence();
          
          Vector3 relativePos =  pPoint - currentPosition;
          Quaternion target = Quaternion.LookRotation(relativePos);
          float a = Quaternion.Angle(_moveTarget.rotation, target);
          Debug.Log("Angle! " + a + " relative rotation: " + target);
          if (a >= StateMachine.PathTracker.RotationMargin)
          {
              Quaternion endRot = _moveTarget.rotation;
              endRot.y -= a;
              _moveTarget.DORotateQuaternion(endRot, a * StateMachine.PathTracker.SecondsPerDegree).SetEase(Ease.Linear);
          }
          /*Vector3 relativePos = _moveTarget.position - StateMachine.PathTracker.TargetPoints[0];
          _rotationTarget = Quaternion.LookRotation(relativePos);
          
          float a = Quaternion.Angle( _rotationTarget, _moveTarget.rotation);
          if (a <= StateMachine.PathTracker.RotationMargin) return;
          
          totalRotationTime =  a * StateMachine.PathTracker.SecondsPerDegree;
          _rotating = true;
          StateMachine.PathTracker.RotationTimer = 0;#1#
    
      } 
      */
     
     public void AddNewRoad(RoadPoints roadPoints)
     {
         if (roadPoints.gameObject == StateMachine.CurrentRoad)
         {
             return;
         }
         StateMachine.PathTracker.UpdateSpeed();
         StateMachine.CurrentRoad = roadPoints.gameObject;
         if (roadPoints.CurvePoints.Count > 0)
         {
             //In a curve, check if its a right or left curve
             if (roadPoints.CurvePointsCross.Count > 0 && !ShouldGoRight())
             {
                 List<TargetPoint> targets = new List<TargetPoint>();
                 for (int i = 0; i < roadPoints.CurvePointsCross.Count; i++)
                 {
                     targets.Add(new TargetPoint(roadPoints.CurvePointsCross[i]));
                 }

                 targets[0].includeInRotation = false;
                 targets[^1].includeInRotation = false;
                 AddToPath(targets);
             }
             else
             {
                 List<TargetPoint> targets = new List<TargetPoint>();
                 for (int i = 0; i < roadPoints.CurvePoints.Count; i++)
                 {
                     targets.Add(new TargetPoint(roadPoints.CurvePoints[i]));
                 }

                 targets[0].includeInRotation = false;
                 targets[^1].includeInRotation = false;
                 AddToPath(targets);
             }
           
         }
         else
         {
             if (StateMachine.PathTracker.TargetPoints.Count > 0)
             {
                 AddToPath(new TargetPoint((roadPoints.AssetStart + ( StateMachine.PathTracker.TargetPoints[0].position - roadPoints.AssetStart )), false));
             }
             else
             {
                 AddToPath(new TargetPoint(roadPoints.AssetStart, false));
             }
             AddToPath(new TargetPoint(roadPoints.AssetEnd));
         }
         SetPath(new List<TargetPoint>());
     }

     protected virtual bool ShouldGoRight()
     {
         return true;
     }
     
 }            

