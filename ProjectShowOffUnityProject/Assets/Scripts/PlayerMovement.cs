using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : StateDependantObject
{
    private Rigidbody _rb;

    [Header("PlayerMovement")] [SerializeField]
    private float _speed;

    [SerializeField]
    private float minX = -2.0f;
    [SerializeField]
    private float maxX = 2.0f;
    private float startX;
    [SerializeField] 
    private string _horizontalAxis = "Horizontal";
    [SerializeField]
    private string _verticalAxis = "Horizontal";
    
    [Header("Jumping")]
    [SerializeField, Tooltip("This is the trajectory of our player, the vertical axis shows the position where the horizontal axis shows where in the 'animation'.")]
    private AnimationCurve _jumpCurve;
    private float startY;
    [SerializeField, Tooltip("This counts as both the intensit of movement and the highest point")] 
    private float _jumpMultiplier = 3;
    [SerializeField] 
    private float _jumpDuration = 2;
    private bool _jumping = false;
    private float _timer = 0;
    
    [Header("Debug")] 
    [SerializeField] private bool _showVisualAid = true;
    [SerializeField] private Color debugCubeColor;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _rb = GetComponent<Rigidbody>();
        startX = transform.position.x;
        startY = transform.position.y;
    }
    /// <summary>
    /// Update but it only calls during certain states
    /// </summary>
    protected override void ReNew()
    {
        base.ReNew();
        float ver = Input.GetAxisRaw(_verticalAxis);
        
        if (ver > 0 && !_jumping)
        {
            //Jump
            _jumping = true;

        }else if (ver < 0)
        {
            
        }
        else
        {
            Move();
        }

        if (_jumping)
        {
            _timer += Time.deltaTime;
            Jump(Mathf.Clamp(1/(_jumpDuration/_timer),0,1));
            if (_timer >= _jumpDuration)
            {
                Vector3 p = transform.position;
                p.y = startY;
                transform.position = p;
                _timer = 0;
                _jumping = false;
            }
        }
        
    }

    private void Jump(float t)
    {
        Vector3 pos = transform.position;
        pos.y = _jumpCurve.Evaluate(t) * _jumpMultiplier;
        transform.position = pos;
    }

    private void Move()
    {
        float hor = Input.GetAxis(_horizontalAxis);
        _rb.velocity = new Vector3(hor, 0, 0) * _speed;
        Vector3 newPos = transform.position;
        newPos.x = Mathf.Clamp(transform.position.x, startX + minX, startX + maxX);
        transform.position = newPos;
    }

    private void OnDrawGizmosSelected()
    {
        if (!_showVisualAid) return;
        
        Vector3 minPos = transform.position;
        minPos.x += minX;
        Vector3 maxPos = transform.position;
        maxPos.x += maxX;
        Gizmos.color = debugCubeColor;
        Gizmos.DrawCube(minPos, Vector3.one);
        Gizmos.DrawCube(maxPos, Vector3.one);
    }
}
