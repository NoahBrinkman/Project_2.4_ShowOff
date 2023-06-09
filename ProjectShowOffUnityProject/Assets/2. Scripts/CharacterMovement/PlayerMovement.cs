using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core.Easing;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(BoxCollider))]
public class PlayerMovement : StateDependantObject<PlayerState>
{
    private Rigidbody _rb;

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

    [field: Header("Roads")]
    [SerializeField] private List<RoadGenerator> biomes;
    [HideInInspector]
    public GameObject CurrentRoad { get; private set; }

    private RoadGenerator _activeRoad;
    private PathFollower _pathFollower;

    [Header("Score")] 
    [SerializeField] private float _baseScoreAddition;
    
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
       // startXZ = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
        startY = transform.position.y;
        startXZ = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
        _activeRoad = biomes[0];
        _activeRoad.IsActive = true;
       
  //     Debug.Log($"Road: {_activeRoad} is {_activeRoad.IsActive}");

        try
        {
            _pathFollower = GetComponentInParent<PathFollower>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }
    /// <summary>
    /// Update but it only calls during certain states
    /// </summary>
    protected override void Run()
    {
        base.Run();
       // Vector3 loopPos = _lookAtPoint.position;
       // loopPos.z = transform.position.z;
       // _lookAtPoint.position = loopPos;
        
        SwitchBiome();
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
            Move();
        }
        
    }
    

     private void Move()
     {
         float hor = Input.GetAxis(_horizontalAxis);
         _rb.velocity = transform.right * hor * _speed;
         Vector3 newPos = transform.position;
         newPos.x = Mathf.Clamp(transform.localPosition.x, startXZ.x + minX, startXZ.x + maxX);
         newPos.z = Mathf.Clamp(transform.localPosition.z, startXZ.z + minX, startXZ.z + maxX);
         transform.localPosition = newPos;
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

     public void SetNewRoad(RoadPoints rP)
     {
         if(CurrentRoad == rP.gameObject) return;
         CurrentRoad = rP.gameObject;
         if (rP.CurvePoints.Count > 0)
         {
             _pathFollower.AddToPath(rP.CurvePoints);
         }
         else
         {
             _pathFollower.AddToPath(rP.AssetStart);
             _pathFollower.AddToPath(rP.AssetEnd);
         }
         _pathFollower.SetPath(new List<Vector3>());
     }
     
    private void SwitchBiome()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            Debug.Log($"Road: {_activeRoad} is {_activeRoad.IsActive}");
            int randomBiome = Random.Range(0, biomes.Count-1);
            _activeRoad.IsActive = false;
            _activeRoad.Clear = true;
            Debug.Log($"Road: {_activeRoad} is {_activeRoad.IsActive}");
            _activeRoad = biomes[1];
            Debug.Log($"Road NEW: {_activeRoad.transform.position} is {_activeRoad.IsActive}");
            Vector3 playersPosition;
            playersPosition = new Vector3(_activeRoad.transform.position.x, 3, _activeRoad.transform.position.z);
            transform.localPosition = playersPosition;
            transform.rotation = _activeRoad.transform.rotation;
            _activeRoad.IsActive = true;
            Debug.Log($"Road: {_activeRoad} is {_activeRoad.IsActive}");
        }
    }

    public void Teleport(Vector3 position, Vector3 forwardDirection)
    {
        transform.parent.position = new Vector3(position.x, transform.parent.position.y, position.z);
        transform.parent.rotation = Quaternion.LookRotation(forwardDirection);

       
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

    public float GetScore()
    {
        float score = 0;
        if (!_pathFollower.Staggered)
        {

            score += _baseScoreAddition;
        }
        //score * 1 + (combo/100);
        return score;
    }
}
