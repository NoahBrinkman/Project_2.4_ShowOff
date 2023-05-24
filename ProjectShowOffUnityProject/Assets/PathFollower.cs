using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    [SerializeField] private float secondsPerUnit = .25f;
    [SerializeField] private List<Vector3> points = new List<Vector3>();
     private List<Vector3> path;
     private float moveTimer;
     private float totalMoveTime;
     private Vector3 oldPosition;
    private bool moving;
    private int currentIndex = 0;
    private bool rotating = false;
    [SerializeField] private float _secondsPerRotation = 1.0f;
    private float rotationTimer;
    private Quaternion rotationTarget;

     private void Start()
     {
         path = new List<Vector3>();
         SetPath(points);
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
         currentIndex = 0;
         moveTimer = 0;
         moving = true;
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
         if (path.Count > 1)
         {
             path.RemoveAt(0);
             ResetRotation(path[0]);
             totalMoveTime = Vector3.Distance(transform.position, path[0]) * secondsPerUnit;
             oldPosition = transform.position;
             currentIndex = 0;
             moveTimer = 0;
         }
         else
         {
            path.RemoveAt(0);
            moving = false;
         }
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
     
     private void MoveFollower()
     {
         if (path.Count >= 1)
         {
             moveTimer += Time.deltaTime;
             Vector3 newPos = Vector3.Lerp(oldPosition, path[0], moveTimer / totalMoveTime);
             transform.position = newPos;
             if (moveTimer >= totalMoveTime)
             {
                 NextMovePoint();
             }
         }
         else
         {
             transform.Translate(-transform.forward * secondsPerUnit * Time.deltaTime);
                 
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
