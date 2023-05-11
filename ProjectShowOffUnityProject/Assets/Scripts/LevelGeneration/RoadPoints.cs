using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class RoadPoints : MonoBehaviour
{
    [SerializeField] private int direction;
    [SerializeField] private int horizontalNumber;
    [SerializeField] private int verticalNumber;

    public int Direction => direction;
    public int HorizontalNumber => horizontalNumber;
    public int VerticalNumber => verticalNumber;

    private Vector3 _assetStart;
    private Vector3 _assetEnd;
    private Vector3 _assetLeft;

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

    private void Start()
    {
        GameObject startPoint = Instantiate(new GameObject(), transform,true);
        startPoint.transform.localPosition = _assetStart;
        startPoint.name = "StartPoint";
        startPoint.transform.parent = transform;
        GameObject endPoint = Instantiate(new GameObject(), _assetEnd, Quaternion.identity);
        endPoint.name = "EndPoint";
        endPoint.transform.parent = transform;
        if (direction == 4)
        {
            GameObject leftPoint = Instantiate(new GameObject(), _assetLeft, Quaternion.identity);
            leftPoint.name = "LeftPoint";
            leftPoint.transform.parent = transform;
        }
    }
}