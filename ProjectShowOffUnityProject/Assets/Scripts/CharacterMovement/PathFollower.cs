using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class PathFollower : StateDependantObject<PlayerState>
{
    [Header("PathFollowing")]
    [SerializeField] private float secondsPerUnit = .25f;
    private List<Vector3> path;
    private float moveTimer;
    private float totalMoveTime;
    private Vector3 oldPosition;
    private bool moving;
    
    [Header("Rotation")]
    [SerializeField] private float _secondsPerRotationUnit = 1.0f;
    [SerializeField, Tooltip("If the rotational distance is less than this: don't bother rotating")] 
    private float _rotationMargin;
    private float rotationTimer;
    private float totalRotationTime;
    private Quaternion rotationTarget;
    private bool rotating = false;
    
    [Header("Stagger")]
    [SerializeField] private float _staggerDistance = 3;
    [SerializeField] private float _staggerDuration = 1;
    [SerializeField] private AnimationCurve _staggerCurveForward;
    [SerializeField] private float staggerMargin; 
    private PlayerMovement _player;
    
    private Vector3 previousPoint;
    protected override void Start()
     {
         base.Start();
         path = new List<Vector3>();
         previousPoint = transform.position;
        // SetPath(points);
         rotationTimer = 0;
         moveTimer = 0;
     }

     public void SetPath(List<Vector3> pPath)
     {
         for (int i = 0; i < pPath.Count; i++)
         {
             path.Add(pPath[i]);
         }
         totalMoveTime = Vector3.Distance(transform.position, path[0]) * secondsPerUnit;
         oldPosition = transform.position;
         moveTimer = 0;
         moving = true;
     }
    
     public void AddToPath(Vector3 pPath)
     {
         if(!path.Contains(pPath))
            path.Add(pPath);
     }
     
     public void AddToPath(List<Vector3> pPath)
     {
         for (int i = 0; i < pPath.Count; i++)
         {
             if(!path.Contains(pPath[i]))
                path.Add(pPath[i]);
         }
     }
     
     public void SetMovingAndRotating(bool acive)
     {
         moving = acive;
         rotating = acive;
     }
     
     private void ResetRotation(Vector3 point)
     {
         Vector3 relativePos =  transform.position - point ;

         rotationTarget = Quaternion.LookRotation(relativePos);
         float a = Quaternion.Angle( rotationTarget, transform.rotation);
         Debug.Log($"Angle: {a}");
         
         if (a <= _rotationMargin) return;
         
         totalRotationTime =  a * _secondsPerRotationUnit;
         rotating = true;
         rotationTimer = 0;
     }
     private void NextMovePoint()
     {
         previousPoint = path[0];
         if (path.Count > 1)
         {
             path.RemoveAt(0);
             ResetRotation(path[0]);
             totalMoveTime = Vector3.Distance(transform.position, path[0]) * secondsPerUnit;
             oldPosition = transform.position;
             moveTimer = 0;
         }
         else
         {
            path.RemoveAt(0);
            
            moving = false;
         }

     }

     public bool ShouldGoRight()
     {
         return _player.transform.localPosition.x < 0;
     }

     private void RotateFollower()
     {
         if (rotating)
         {
             rotationTimer += Time.deltaTime;
             transform.rotation = Quaternion.Slerp(transform.rotation, rotationTarget,  rotationTimer / totalRotationTime);
     
             if (rotationTimer >= totalRotationTime)
             {
                 rotating = false;
                 rotationTimer = 0;
             }
         }
     }

     public void StaggerBackThenGoBackToMoving()
     {
         PlayerStateMachine sm = GetComponent<PlayerStateMachine>();
         Vector3 endValue = (transform.position + transform.forward * _staggerDistance);
         
         
         
         float percentageTravlled = (endValue - previousPoint).magnitude / (path[0] - previousPoint).magnitude;
        Debug.Log(percentageTravlled);
         sm.SwitchState(sm.GetState<PlayerStaggerState>());
         if (percentageTravlled <= staggerMargin)
         {
             moveTimer = 0;
             transform.DOMove(previousPoint, _staggerDuration + .1f).SetEase(_staggerCurveForward).OnComplete(
                 delegate (){  sm.SwitchState(sm.GetState<PlayerMoveState>());});
         }
         else
         {
             moveTimer = percentageTravlled;
             transform.DOMove(endValue, _staggerDuration + .1f).SetEase(_staggerCurveForward).OnComplete(
                 delegate (){  sm.SwitchState(sm.GetState<PlayerMoveState>());});
         }
     }
     
     private void MoveFollower()
     {
         if (path.Count >= 1)
         {
             moveTimer += Time.deltaTime;
             Vector3 newPos = Vector3.LerpUnclamped(oldPosition, path[0], moveTimer / totalMoveTime);
             transform.position = newPos;
             if (moveTimer >= totalMoveTime)
             {
                 NextMovePoint();
             }
         }
         else
         {
            // transform.Translate(-transform.forward * secondsPerUnit * Time.deltaTime);
         }
     }
     
     private void Update()
     {
         if (moving)
         {
             MoveFollower();
         }

         if (rotating)
         {
             RotateFollower();
         }
     }


     private void OnDrawGizmos()
     {
         
         if (moving)
         {
             if (path.Count > 0)
             {
                 for (int i = 0; i < path.Count; i++)
                 {
                     Gizmos.color =Color.green;
                    Gizmos.DrawSphere(path[i],.2f);
                 }
             }
         }
     }
}
