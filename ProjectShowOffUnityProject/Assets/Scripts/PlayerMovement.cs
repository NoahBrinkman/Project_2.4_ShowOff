using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core.Easing;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(BoxCollider))]
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


    [SerializeField] private Transform _lookAtPoint;
    public Transform LookAtPoint => _lookAtPoint;
    [SerializeField] private Transform _virtualCameraTransform;
    
    [Header("Jumping")]
    [SerializeField, Tooltip("This is the trajectory of our player, the vertical axis shows the position where the horizontal axis shows where in the 'animation'.")]
    private AnimationCurve _jumpCurve;
    private float startY;
    [FormerlySerializedAs("_jumpMultiplier")] [SerializeField, Tooltip("This counts as both the intensit of movement and the highest point")] 
    private float _jumpHeight = 3;
    [SerializeField] 
    private float _jumpDuration = 2;
    [SerializeField] private float _jumpCancelSpeedMultiplier = 10;
    private bool _jumping = false;
    private Tween jumpTween;
    
    [Header("Sliding")] 
    [SerializeField] private AnimationCurve _slideCurve;
    [SerializeField] private Vector3 _slideLowestPointCentre;
    [SerializeField] private Vector3 _slideLowestPointSize;
    [SerializeField] private float _slideDuraton;
    [SerializeField] private float _slideCancelSpeedMultiplier = 10;
    private bool _sliding = false;
    private List<Tween> slideTweens;
    
    [Header("Obstacles")] 
    [SerializeField] private LayerMask _obstacleLayer = 8;
    [SerializeField] private UnityEvent _onObstacleHit;
    private BoxCollider _collider;
    
    [Header("Debug")] 
    [SerializeField] private bool _showVisualAid = true;
    [SerializeField, Tooltip("In case the player has no model. Use this")] private Transform _debugCube;
    [FormerlySerializedAs("debugCubeColor")] [SerializeField] private Color _debugCubeColor;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<BoxCollider>();
        slideTweens = new List<Tween>();
        startX = transform.position.x;
        startY = transform.position.y;
    }
    /// <summary>
    /// Update but it only calls during certain states
    /// </summary>
    protected override void ReNew()
    {
        base.ReNew();
        Vector3 loopPos = _lookAtPoint.position;
        loopPos.z = transform.position.z;
        _lookAtPoint.position = loopPos;
        if (Input.GetKey(KeyCode.W))
        {
            _rb.velocity = transform.forward * _speed;
        }
        else
        {
            _rb.velocity = new Vector3();
        }
        float ver = Input.GetAxisRaw(_verticalAxis);
        
        if (ver > 0 && !_jumping)
        {
            if (_sliding)
            {
                //jumpTween.timeScale *= 10;
                for (int i = 0; i < slideTweens.Count; i++)
                {
                    slideTweens[i].timeScale = _slideCancelSpeedMultiplier;
                }
            }
            //Jump
            _jumping = true;
           jumpTween = transform.DOMoveY(_jumpHeight, _jumpDuration)
                .SetEase(_jumpCurve).OnComplete(delegate() { _jumping = false;
                    jumpTween.timeScale = 1;
                });

        }else if (ver < 0 && !_sliding)
        {
            
            if (_jumping)
            {
                jumpTween.timeScale = _jumpCancelSpeedMultiplier;
            }
            _sliding = true;
            slideTweens.Add(
            DOTween.To(() => _collider.center, x => _collider.center =  x, _slideLowestPointCentre, _slideDuraton)
                .SetEase(_slideCurve).OnComplete(delegate()
                {
                    _sliding = false;
                    slideTweens = new List<Tween>();
                }));
           slideTweens.Add( DOTween.To(() => _collider.size, x => _collider.size = x, _slideLowestPointSize, _slideDuraton)
                .SetEase(_slideCurve));
           
            if (_debugCube != null)
            {
                slideTweens.Add( _debugCube.transform.DOLocalMove(_slideLowestPointCentre, _slideDuraton).SetEase(_slideCurve));
                slideTweens.Add( _debugCube.transform.DOScale(_slideLowestPointSize, _slideDuraton).SetEase(_slideCurve));
            }
        }
        else
        {
            //Move();
        }
        
    }
    
    private void Move()
    {
        float hor = Input.GetAxis(_horizontalAxis);
        _rb.velocity = new Vector3(hor, 0, 0) * _speed;
        Vector3 newPos = transform.position;
        newPos.x = Mathf.Clamp(transform.position.x, startX + minX, startX + maxX);
        transform.position = newPos;
    }
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log($"Hit {collision.gameObject.name}");
        if (     _obstacleLayer == (_obstacleLayer | (1 << collision.gameObject.layer)))
        {
            _onObstacleHit?.Invoke();
            Debug.Log($"{gameObject.name} hit an obstacle");
        }
    }

    public void Teleport(Vector3 position, Vector3 forwardDirection)
    {
        transform.position = new Vector3(position.x, transform.position.y, position.z);
        _lookAtPoint.position = new Vector3(position.x, _lookAtPoint.position.y, position.z);
        transform.rotation = Quaternion.LookRotation(forwardDirection);
        _lookAtPoint.rotation = Quaternion.LookRotation(-forwardDirection);
       
    }

    private void OnDrawGizmosSelected()
    {
        if (!_showVisualAid) return;
        
        
        
        Vector3 minPos = transform.position;
        minPos.x += minX;
        Vector3 maxPos = transform.position;
        maxPos.x += maxX;
        Gizmos.color = _debugCubeColor;
        Gizmos.DrawCube(minPos, Vector3.one);
        Gizmos.DrawCube(maxPos, Vector3.one);
        Gizmos.DrawWireCube(transform.position + _slideLowestPointCentre, _slideLowestPointSize);
    }

}
