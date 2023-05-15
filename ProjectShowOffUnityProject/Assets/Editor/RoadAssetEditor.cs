using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

[CustomEditor(typeof(RoadPoints))]
public class RoadAssetEditor : Editor
{
    private RoadPoints _road;
    [SerializeField] private Vector3 _assetStartEditor;
    private Vector3 _assetEnd;
    private Vector3 _assetLeft;
    private Vector3 _assetRight;

    private Bounds _bounds;

    private void OnEnable()
    {
        _road = (RoadPoints)target;
    }

    private void OnSceneGUI()
    {
        _bounds = _road.GetComponent<Renderer>().bounds;

        float roadRotation = _road.transform.localRotation.eulerAngles.y;
        int direction = _road.Direction;
        int verticalNumber = _road.VerticalNumber;
        int horizontalNumber = _road.HorizontalNumber;
        //Debug.Log(roadRotation);

        if (direction == 1)
        {
            if (roadRotation == 0)
            {
                SpawnPoints(_bounds.size.x / 2 - _bounds.size.x / verticalNumber, -_bounds.size.z / 2,
                    0, _bounds.size.z / 2);
            }
            else if (roadRotation == 180)
            {
                SpawnPoints(0,_bounds.size.z / 2, 0, -_bounds.size.z/2);
            }
            else if (roadRotation == 90)
            {
                SpawnPoints(-_bounds.size.x / 2,0,_bounds.size.x/2,0);
            }
            else if (roadRotation == 270)
            {
                SpawnPoints(_bounds.size.x/2,0,-_bounds.size.x/2, 0);
            }
        }
        else if (direction == 2)
        {
            if (roadRotation == 0)
            {
                SpawnPoints(_bounds.size.x / 2 - _bounds.size.x / verticalNumber,- _bounds.size.z / 2,
                    -_bounds.size.x / 2,_bounds.size.z / 2 - _bounds.size.z / horizontalNumber);
            }
            else if (roadRotation == 180)
            {
                SpawnPoints(-_bounds.size.x / 2 + _bounds.size.x / verticalNumber, _bounds.size.z / 2,
                    _bounds.size.x / 2, -_bounds.size.z / 2 + _bounds.size.z / horizontalNumber);
            }
            else if (roadRotation == 90)
            {
                SpawnPoints(-_bounds.size.x / 2, -_bounds.size.z / 2 + _bounds.size.z / verticalNumber,
                    _bounds.size.x / 2 - _bounds.size.x / horizontalNumber,_bounds.size.z / 2);
            }
            else if (roadRotation == 270)
            {
                SpawnPoints(_bounds.size.x / 2, _bounds.size.z / 2 - _bounds.size.z / verticalNumber,
                    -_bounds.size.x / 2 + _bounds.size.x / horizontalNumber,-_bounds.size.z / 2);
            }
        }
        else if (direction == 3)
        {
            if (roadRotation == 0)
            {
                SpawnPoints(-_bounds.size.x / 2 + _bounds.size.x / verticalNumber, -_bounds.size.z/2,
                    _bounds.size.x / 2, _bounds.size.z / 2 - _bounds.size.z / horizontalNumber);
            }
            else if (roadRotation == 180)
            {
                SpawnPoints(_bounds.size.x / 2 - _bounds.size.x / verticalNumber, _bounds.size.z/2,
                    -_bounds.size.x / 2, -_bounds.size.z / 2 + _bounds.size.z / horizontalNumber);
            }
            else if (roadRotation == 90)
            {
                SpawnPoints(-_bounds.size.x/2, _bounds.size.z / 2 - _bounds.size.z / verticalNumber,
                    _bounds.size.x / 2 - _bounds.size.x / horizontalNumber, -_bounds.size.z / 2);
            }
            else if (roadRotation == 270)
            {
                SpawnPoints(_bounds.size.x / 2, -_bounds.size.z / 2 + _bounds.size.z / verticalNumber,
                    -_bounds.size.x / 2 + _bounds.size.x / horizontalNumber, _bounds.size.z / 2);
            }
        }
        else if (direction == 4)
        {
            if (roadRotation == 0)
            {
                SpawnPoints(0, -_bounds.size.z/2,
                    _bounds.size.x / 2, _bounds.size.z / 2 - _bounds.size.z / horizontalNumber,
                    true, -_bounds.size.x / 2, _bounds.size.z / 2 - _bounds.size.z / horizontalNumber);
            }
            else if (roadRotation == 180)
            {
                SpawnPoints(0,_bounds.size.z / 2,
                    -_bounds.size.x / 2, -_bounds.size.z / 2 + _bounds.size.z / horizontalNumber,
                    true, _bounds.size.x / 2, -_bounds.size.z / 2 + _bounds.size.z / horizontalNumber);
            }
            else if (roadRotation == 90)
            {
                SpawnPoints(-_bounds.size.x / 2, 0,
                    _bounds.size.x / 2 - _bounds.size.x / horizontalNumber, -_bounds.size.z / 2,
                    true, _bounds.size.x / 2 - _bounds.size.x / horizontalNumber,_bounds.size.z / 2);
            }
            else if (roadRotation == 270)
            {
                SpawnPoints(_bounds.size.x / 2,0,
                    -_bounds.size.x / 2 + _bounds.size.x / horizontalNumber,_bounds.size.z / 2,
                    true, -_bounds.size.x / 2 + _bounds.size.x / horizontalNumber,-_bounds.size.z / 2);
            }
        }
    }

    private void SpawnPoints(float xOffsetStart, float zOffsetStart, float xOffsetEnd, float zOffsetEnd,
        bool isCrossroad = false, float xOffsetLeft = 0, float zOffsetLeft = 0)
    {
        _assetStartEditor = new Vector3(_bounds.center.x + xOffsetStart, _bounds.center.y - _bounds.size.y / 2,
            _bounds.center.z + zOffsetStart);
        _assetEnd = new Vector3(_bounds.center.x + xOffsetEnd, _bounds.center.y - _bounds.size.y / 2,
            _bounds.center.z + zOffsetEnd);

        if (isCrossroad)
        {
            _assetLeft = new Vector3(_bounds.center.x + xOffsetLeft,_bounds.center.y - _bounds.size.y / 2,
                _bounds.center.z + zOffsetLeft);
            Handles.color = new Color(1, 0.6f, 0);
            Handles.DrawSolidDisc(_assetLeft, Vector3.up, 0.2f);
        }

        Handles.color = Color.green;
        Handles.DrawSolidDisc(_assetStartEditor, Vector3.up, 0.2f);
        Handles.color = Color.red;
        Handles.DrawSolidDisc(_assetEnd, Vector3.up, 0.2f);
    }
}