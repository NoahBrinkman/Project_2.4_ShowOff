using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerMovement : StateDependantObject
{
    private Rigidbody _rb;

    [SerializeField] private float _speed;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!state.Active) return;
        
        float ver = Input.GetAxis("Vertical");
        float hor = Input.GetAxis("Horizontal");
        _rb.velocity = new Vector3(hor, 0, ver) * _speed;
    }
}
