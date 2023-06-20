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


    [SerializeField] 
    private string _horizontalAxis = "Horizontal";

    [Header("Grinding")]
    [SerializeField] private float angleOffset = 5;
    [SerializeField] private float angleLimit = 30;
    [SerializeField] private float speedOfRotation = 10;
    [SerializeField] private int timeToSubstractLife;
    [SerializeField] private float _disBalanceSeverity = 10;
    private bool _staggered = false;
    private float _angle;
    private Vector3 startXZ;
    private int _lostBalance;
    private float disBalanceCounter = 0;
    private void Start()
    {
        _col.OnCollisionEnterEvent.AddListener(OnCollision);
        startXZ = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
    }


    public override void Enter()
    {
        base.Enter();
        Debug.Log("I AM GRINDING YESSS");
        _staggered = false;
        _moving = true;
        Vector3 newPos = _rb.transform.localPosition;
        newPos.x = Mathf.Clamp(_rb.transform.localPosition.x, startXZ.x, startXZ.x);
        newPos.z = Mathf.Clamp(_rb.transform.localPosition.z, startXZ.z, startXZ.z);
        _rb.transform.localPosition = newPos;
    }


    public override void Run()
    {
        base.Run();
        AddDisbalance();
        Balance();
    }

    private void AddDisbalance()
    {
        float angle = Random.Range(0,_disBalanceSeverity);
        disBalanceCounter+= 0.1f;

        angle = Mathf.Sin(disBalanceCounter) * (_disBalanceSeverity/2 +Random.Range(-_disBalanceSeverity/2, _disBalanceSeverity/2)) ;
        Vector3 position = new Vector3(_moveTarget.position.x, _moveTarget.position.y, _moveTarget.position.z);
        _moveTarget.RotateAround(position,new Vector3(1,0,0), angle*Time.deltaTime);
        
    }
    private void Balance()
    {
        float hor = Input.GetAxis(_horizontalAxis);
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
        _angle = Mathf.Clamp(_angle, -angleLimit+speedOfRotation, angleLimit-speedOfRotation);
        Vector3 position = new Vector3(_moveTarget.position.x, _moveTarget.position.y, _moveTarget.position.z);
        _moveTarget.RotateAround(position,new Vector3(1,0,0), _angle*Time.deltaTime);
        float playerRotation = (_col.gameObject.transform.eulerAngles.z + 360) % 360;
        if (playerRotation > angleLimit && playerRotation < 360-angleLimit)
        {
            _lostBalance++;
        }

        if (_lostBalance > timeToSubstractLife)
        {
            _lostBalance = 0;
            StateMachine.SubtractLife(1);
        }
    }

    private void OnCollision(Collider info)
    {
        if (!Active || _staggered) return;

        if (!info.gameObject.CompareTag("Grind"))
        {
            if(Active)
                StateMachine.SwitchState(StateMachine.GetState<PlayerMoveRunningState>());
        }
    }
    
}
