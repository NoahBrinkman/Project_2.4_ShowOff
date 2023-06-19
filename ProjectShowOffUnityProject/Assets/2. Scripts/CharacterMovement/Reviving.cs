using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Reviving : MonoBehaviour
{
    [SerializeField] private GameObject left;
    [SerializeField] private GameObject right;
    [SerializeField] private bool enableReviving = true;
    [SerializeField] private int reviveTime = 30;
    
    private Renderer _leftRender;
    private Renderer _rightRender;
    private int _value;
    

    private void Start()
    {
        _leftRender = left.GetComponent<Renderer>();
        _rightRender = right.GetComponent<Renderer>();
    }
    
    void Update()
    {
        if (enableReviving)
        {
            if (Input.GetKeyUp(KeyCode.A) && _value % 2 == 0)
            {
                _value++;
                _leftRender.material.color = Color.red;
                _rightRender.material.color = Color.white;
            } else if (Input.GetKeyUp(KeyCode.D) && _value % 2 == 1)
            {
                _value++;
                _rightRender.material.color = Color.red;
                _leftRender.material.color = Color.white;
            }
        }
        

        if (_value >= reviveTime)
        {
            Debug.LogWarning("YOU REVIVED!!!");
            enableReviving = false;
        }
    }

    
    
    
}
