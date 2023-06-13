using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickPathSupplier : MonoBehaviour
{
    [SerializeField] private List<Vector3> _path;
    [SerializeField] private PlayerMoveRunningState _runner;


    private void Start()
    {
        //_runner.SetPath(_path);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0,1,1,.25f);
       
        if(_path.Count <= 0) return;
        
        for (int i = 0; i < _path.Count; i++)
        {
            Gizmos.DrawSphere(_path[i],1);    
        }
    }
}
