using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlayerGrindingState : PlayerOnTrackState
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


    private RoadPoints _activeRoad;
    private float _angle;
    [SerializeField] private float angleOffset = 5;
    [SerializeField] private float angleLimit = 30;
    
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
        Debug.Log("I AM GRINDING YESSS");
        _activeRoad = StateMachine.CurrentRoad.GetComponent<RoadPoints>();
        _staggered = false;
        _moving = true;
    }


    public override void Run()
    {
        base.Run();
        //MoveAlongPath();
        //MoveHorizontal();
        Balance();
    }

    private void Balance()
    {
        float hor = Input.GetAxis(_horizontalAxis);
        //transform.RotateAround(_col.transform.position,new Vector3(1,0,0), 5*Time.deltaTime);
        Debug.Log($"Horizontal axis is: {hor}");
        if (Mathf.Approximately(hor,0.0f))
        {
            int random = Random.Range(0, 51);
            random %= 2;
            if (random == 0)
            {
                _angle -= angleOffset;
            }
            else
            {
                _angle += angleOffset;
            }
        }
        else
        {
            if (hor > 0)
            {
                _angle += angleOffset;
            }
            else
            {
                _angle -= angleOffset;
            }
        }
        _angle = Mathf.Clamp(_angle, -angleLimit, angleLimit);
        _col.transform.RotateAround(_col.transform.position,new Vector3(1,0,0), _angle*Time.deltaTime);
    }

    private void OnCollision(Collider info)
    {
        if (!Active || _staggered) return;

        if (!info.gameObject.CompareTag("Grind"))
        {
            _col.transform.RotateAround(_col.transform.position,new Vector3(1,0,0), 0);
            if(Active)
                StateMachine.SwitchState(StateMachine.GetState<PlayerMoveRunningState>());
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
