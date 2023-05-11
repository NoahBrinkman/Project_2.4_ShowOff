using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class RoadPoints : MonoBehaviour
{

    private Vector3 _assetStart;

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

    private Vector3 _assetEnd;

    private void Update()
    {
        //Debug.Log("Rotation : " + transform.localRotation.y);
    }
}
