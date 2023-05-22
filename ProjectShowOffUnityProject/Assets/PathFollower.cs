using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    [SerializeField] private float secondsTillEndOfRoadSegment;
    [SerializeField] private float reduceAfterComplete = 0.01f;
   [SerializeField] private List<Vector3> points = new List<Vector3>();
    
   
   
    private Vector3 currentTarget;

    private void Start()
    {
        SetPath(points);
    }


    public void SetPath(List<Vector3> points)
    {
        
        transform.DOPath(points.ToArray(), secondsTillEndOfRoadSegment, PathType.Linear).SetLookAt(0.01f).OnComplete(
            delegate
            {
                Debug.Log("Hi");
                MoveForward();
            });
       
      
    }

    public void MoveForward()
    {
        transform.DOMoveZ(transform.position.z + 20, 6);
    }
}
