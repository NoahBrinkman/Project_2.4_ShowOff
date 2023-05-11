using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RoadPoints))]
public class RoadAssetEditor : Editor
{
    private RoadPoints _road;
    private Vector3 _assetStart;
    private Vector3 _assetEnd;
    private Vector3 _assetLeft;
    private Vector3 _assetRight;

    private Bounds _bounds;

    private void OnEnable()
    {
        _road = (RoadPoints)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

    private void OnSceneGUI()
    {
        _bounds = _road.GetComponent<Renderer>().bounds;

        float roadRotation = _road.transform.localRotation.eulerAngles.y;
        //Debug.Log(roadRotation);

        if (roadRotation == 0)
        {
            _assetStart = new Vector3(_bounds.center.x, _bounds.center.y - _bounds.size.y / 2,
                _bounds.center.z - _bounds.size.z / 2);
            _assetEnd = new Vector3(_bounds.center.x, _bounds.center.y - _bounds.size.y / 2,
                _bounds.center.z + _bounds.size.z / 2);
        }
        else if (roadRotation == 180)
        {
            _assetStart = new Vector3(_bounds.center.x, _bounds.center.y - _bounds.size.y / 2,
                _bounds.center.z + _bounds.size.z / 2);
            _assetEnd = new Vector3(_bounds.center.x, _bounds.center.y - _bounds.size.y / 2,
                _bounds.center.z - _bounds.size.z / 2);
        }
        else if (roadRotation == 90)
        {
            _assetStart = new Vector3(_bounds.center.x - _bounds.size.x / 2, _bounds.center.y - _bounds.size.y / 2,
                _bounds.center.z);
            _assetEnd = new Vector3(_bounds.center.x + _bounds.size.x / 2, _bounds.center.y - _bounds.size.y / 2,
                _bounds.center.z);
        }
        else if (roadRotation == 270)
        {
            _assetStart = new Vector3(_bounds.center.x + _bounds.size.x / 2, _bounds.center.y - _bounds.size.y / 2,
                _bounds.center.z);
            _assetEnd = new Vector3(_bounds.center.x - _bounds.size.x / 2, _bounds.center.y - _bounds.size.y / 2,
                _bounds.center.z);
        }
        
        Handles.color = Color.green;
        Handles.DrawSolidDisc(_assetStart, Vector3.up, 0.2f);
        Handles.color = Color.red;
        Handles.DrawSolidDisc(_assetEnd, Vector3.up, 0.2f);
    }
}