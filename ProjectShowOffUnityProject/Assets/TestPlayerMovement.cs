using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerMovement : StateDependantObject
{
    private Rigidbody _rb;
    [Header("PlayerMovement")]
    [SerializeField] private float _speed;

    [SerializeField] private float minX = -2.0f;
    [SerializeField] private float maxX = 2.0f;
    private float startX;
    [SerializeField] private string _horizontalAxis = "Horizontal";
    [SerializeField] private string _verticalAxis = "Horizontal";

    [Header("Debug")] [SerializeField] private bool _showVisualAid = true;

    [SerializeField] private Color sphereColor;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _rb = GetComponent<Rigidbody>();
        startX = transform.position.x;

    }
    /// <summary>
    /// Update but it only calls during certain states
    /// </summary>
    protected override void ReNew()
    {
        base.ReNew();
        float ver = Input.GetAxis(_verticalAxis);
        float hor = Input.GetAxis(_horizontalAxis);
        _rb.velocity = new Vector3(hor, 0, ver) * _speed;
        Vector3 newPos = transform.position;
        newPos.x = Mathf.Clamp(transform.position.x, startX + minX, startX + maxX);
        transform.position = newPos;
    }


    private void OnDrawGizmosSelected()
    {
        if (!_showVisualAid) return;
        
        Vector3 minPos = transform.position;
        minPos.x += minX;
        minPos.y -= .5f;
        Vector3 maxPos = transform.position;
        maxPos.x += maxX;
        maxPos.y -= .5f;
        Gizmos.color = sphereColor;
        Gizmos.DrawSphere(minPos, .2f);
        Gizmos.DrawSphere(maxPos, .2f);
    }
}
