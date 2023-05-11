using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class RoadPoints : MonoBehaviour
{
    [SerializeField] private GameObject startPoint;
    [SerializeField] private GameObject endPoint;
    [SerializeField] private GameObject leftPoint;
    [SerializeField] private int direction;
    [SerializeField] private int horizontalNumber;
    [SerializeField] private int verticalNumber;

    public int Direction => direction;
    public int HorizontalNumber => horizontalNumber;
    public int VerticalNumber => verticalNumber;

    [SerializeField] private Vector3 _assetStart;
    private Vector3 _assetEnd;
    private Vector3 _assetLeft;
    private Bounds _bounds;

    public Vector3 AssetStart
    {
        get => _assetStart;
        set => _assetStart = value;
    }

    public Vector3 AssetEnd
    {
        get => _assetEnd;
        set => _assetEnd = value;
    }

    public Vector3 AssetLeft
    {
        get => _assetLeft;
        set => _assetLeft = value;
    }


    private void Awake()
    {
        _bounds = GetComponent<Renderer>().bounds;
    }

    private void Start()
    {
        float roadRotation = transform.localRotation.eulerAngles.y;
        
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
        
        GameObject point1 = Instantiate(startPoint, _assetStart, Quaternion.identity);
        point1.name = "StartPoint";
        point1.transform.parent = transform;
        
        GameObject point2 = Instantiate(endPoint, _assetEnd, Quaternion.identity);
        point2.name = "EndPoint";
        point2.transform.parent = transform;

        if (direction == 4)
        {
            GameObject point3 = Instantiate(leftPoint, _assetLeft, Quaternion.identity);
            point3.name = "LeftPoint";
            point3.transform.parent = transform;
            
        }

    }
    
    private void SpawnPoints(float xOffsetStart, float zOffsetStart, float xOffsetEnd, float zOffsetEnd,
        bool isCrossroad = false, float xOffsetLeft = 0, float zOffsetLeft = 0)
    {
        _assetStart = new Vector3(_bounds.center.x + xOffsetStart, _bounds.center.y - _bounds.size.y / 2,
            _bounds.center.z + zOffsetStart);
        _assetEnd = new Vector3(_bounds.center.x + xOffsetEnd, _bounds.center.y - _bounds.size.y / 2,
            _bounds.center.z + zOffsetEnd);

        if (isCrossroad)
        {
            _assetLeft = new Vector3(_bounds.center.x + xOffsetLeft,_bounds.center.y - _bounds.size.y / 2,
                _bounds.center.z + zOffsetLeft);
        }
    }
}