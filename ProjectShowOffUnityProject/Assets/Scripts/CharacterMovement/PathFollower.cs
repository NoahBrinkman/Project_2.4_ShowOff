using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PathFollower : StateDependantObject<PlayerState>
{
    [SerializeField] private float secondsPerUnit = .25f;
    [SerializeField] private List<Vector3> points = new List<Vector3>();
     private List<Vector3> path;
     private float moveTimer;
     private float totalMoveTime;
     private Vector3 oldPosition;
    private bool moving;
    private bool rotating = false;
    [SerializeField] private float _secondsPerRotation = 1.0f;
    private float rotationTimer;
    private Quaternion rotationTarget;
    
    [SerializeField] private float _staggerDistance = 3;
    [SerializeField] private float _staggerDuration = 1;
    [SerializeField] private AnimationCurve _staggerCurveForward;

    private PlayerMovement _player;
    
    private Vector3 previousPoint;
    protected override void Start()
     {
         base.Start();
         path = new List<Vector3>();
         previousPoint = transform.position;
         //SetPath(points);
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
         path.Add(pPath);
     }
     
     public void AddToPath(List<Vector3> pPath)
     {
         for (int i = 0; i < pPath.Count; i++)
         {
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
             transform.rotation = Quaternion.Slerp(transform.rotation, rotationTarget,  rotationTimer / _secondsPerRotation);
     
             if (rotationTimer >= _secondsPerRotation)
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
         moveTimer = percentageTravlled;
         sm.SwitchState(sm.GetState<PlayerStaggerState>());
         transform.DOMove(endValue, _staggerDuration + .1f).SetEase(_staggerCurveForward).OnComplete(
             delegate (){  sm.SwitchState(sm.GetState<PlayerMoveState>());});
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
         for (int i = 0; i < points.Count; i++)
         {
             Gizmos.color = Color.blue;
             
             Gizmos.DrawSphere(points[i], .5f);
         }
     }
}
