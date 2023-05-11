using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerMovement : StateDependantObject
{
    private Rigidbody _rb;

    [SerializeField] private float _speed;

    [SerializeField] private string _horizontalAxis = "Horizontal";
    [SerializeField] private string _verticalAxis = "Horizontal";
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _rb = GetComponent<Rigidbody>();
        
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
    }
    
}
