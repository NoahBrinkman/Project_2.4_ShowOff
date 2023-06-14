using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlayerMoveRunningState : PlayerOnTrackState
{
    
    [Header("References")]
    [SerializeField] private EventCollider _col;
    [SerializeField] private Rigidbody _rb;
    
    
    [Header("PlayerMovement")] [SerializeField]
    private float _speed;
    [SerializeField]
    private float minX = -2.0f;
    [SerializeField]
    private float maxX = 2.0f;
    private Vector3 startXZ;
   
    [SerializeField] 
    private string _horizontalAxis = "Horizontal";
    [SerializeField]
    private string _verticalAxis = "Vertical";
    
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
    private bool _staggered = false;
    [Header("Debug")] 
    [SerializeField] private bool _showVisualAid = true;
    [SerializeField, Tooltip("In case the player has no model. Use this")] private Transform _debugCube;
    [FormerlySerializedAs("debugCubeColor")] [SerializeField] private Color _debugCubeColor;
    
    private void Start()
    {
        _col.OnCollisionEnterEvent.AddListener(OnCollision);
        startY = transform.position.y;
        startXZ = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
        slideTweens = new List<Tween>();
    }


    public override void Enter()
    {
        base.Enter();
        _staggered = false;
    }


    public override void Run()
    {
        base.Run();
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
            jumpTween = _col.transform.DOMoveY(_jumpHeight, _jumpDuration)
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
                DOTween.To(() => _col.Collider.center, x => _col.Collider.center =  x, _slideLowestPointCentre, _slideDuraton)
                    .SetEase(_slideCurve).OnComplete(delegate()
                    {
                        _sliding = false;
                        slideTweens = new List<Tween>();
                    }));
            slideTweens.Add( DOTween.To(() => _col.Collider.size, x => _col.Collider.size = x, _slideLowestPointSize, _slideDuraton)
                .SetEase(_slideCurve));
           
            if (_debugCube != null)
            {
                slideTweens.Add( _debugCube.transform.DOLocalMove(_slideLowestPointCentre, _slideDuraton).SetEase(_slideCurve));
                slideTweens.Add( _debugCube.transform.DOScale(_slideLowestPointSize, _slideDuraton).SetEase(_slideCurve));
            }
        }
        else
        {
            MoveHorizontal();
        }
    }
    
    
    private void MoveHorizontal()
    {
        float hor = Input.GetAxis(_horizontalAxis);
        Vector3 vel = _rb.transform.right * hor * _speed;
        _rb.velocity = vel;
        Vector3 newPos = _rb.transform.localPosition;
        newPos.x = Mathf.Clamp(_rb.transform.localPosition.x, startXZ.x + minX, startXZ.x + maxX);
        newPos.z = Mathf.Clamp(_rb.transform.localPosition.z, startXZ.z + minX, startXZ.z + maxX);
        _rb.transform.localPosition = newPos;
    }
    
    private void OnCollision(Collider info)
    {
        if (!Active || _staggered) return;
        if (_obstacleLayer == (_obstacleLayer | (1 << info.gameObject.layer)))
        {
            _onObstacleHit?.Invoke();
            _staggered = true;
        }

        if (info.gameObject.CompareTag("Grind"))
        {
            if(Active)
                StateMachine.SwitchState(StateMachine.GetState<PlayerGrindingState>());
        }
    }

    protected override bool ShouldGoRight()
    {
        return _rb.transform.localPosition.x < 0;
    }

    private void OnDrawGizmosSelected()
    {
        if (!_showVisualAid) return;
        
        
        
        Gizmos.color = _debugCubeColor;
        Vector3 minPos = transform.position - (transform.right * minX);
        
        Vector3 maxPos = transform.position - (transform.right * maxX);
       
        Gizmos.DrawCube(minPos, Vector3.one);
        Gizmos.DrawCube(maxPos, Vector3.one);
        Gizmos.DrawWireCube(transform.position + _slideLowestPointCentre, _slideLowestPointSize);
    }
}
