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
    [SerializeField] private float timeToSubstractLife;
    [SerializeField] private float _disBalanceSeverity = 10;
    [SerializeField] private KeyCode playerLeft = KeyCode.C;
    [SerializeField] private KeyCode playerRight = KeyCode.N;
    private bool _staggered = false;
    private float _angle;
    private Vector3 startXZ;
    private float _lostBalance;
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
        _rb.velocity = new Vector3();
        _rb.transform.localPosition = new Vector3(0,.5f,0);
        // Vector3 newPos = _rb.transform.localPosition;
        //   newPos.x = Mathf.Clamp(_rb.transform.localPosition.x, startXZ.x, startXZ.x);
        //   newPos.z = Mathf.Clamp(_rb.transform.localPosition.z, startXZ.z, startXZ.z);
        //  _rb.transform.localPosition = newPos;
    }


    public override void Run()
    {
        base.Run();
        AddDisbalance();
        Balance();
        if (Input.GetKeyDown(KeyCode.Y))
        {
            StateMachine.SubtractLife(1, this);
        }
    }
    
    private void FixedUpdate()
    {
        if(Active) ScoreManager.Instance.AddScore(StateMachine);
    }
    
    private void AddDisbalance()
    {
        float angle = Random.Range(0,_disBalanceSeverity);
        disBalanceCounter+= 0.1f;

        angle = Mathf.Sin(disBalanceCounter) * (_disBalanceSeverity/2 +Random.Range(-_disBalanceSeverity/2, _disBalanceSeverity/2)) ;
        Vector3 position = new Vector3(_moveTarget.position.x, _moveTarget.position.y, _moveTarget.position.z);
        _moveTarget.RotateAround(position, _moveTarget.forward, angle*Time.deltaTime*10);
        
    }
    private void Balance()
    {
        // float hor = Input.GetAxis(_horizontalAxis);
        // if (Mathf.Approximately(hor,0.0f))
        // {
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
        // }
        // else
        // {
            if (Input.GetKeyUp(playerRight))
            {
                _angle += angleOffset;
            }
            else if (Input.GetKeyUp(playerLeft))
            {
                _angle -= angleOffset;
            }
        //}
        _angle = Mathf.Clamp(_angle, -angleLimit+speedOfRotation, angleLimit-speedOfRotation);
        Vector3 position = new Vector3(_moveTarget.position.x, _moveTarget.position.y, _moveTarget.position.z);
        _moveTarget.RotateAround(position, _moveTarget.forward, _angle*Time.deltaTime);
        float playerRotation = (_col.gameObject.transform.eulerAngles.z + 360) % 360;
//        Debug.Log($"Player rotation: {playerRotation}  Angle limit: {angleLimit}");
      //  Debug.Log($"Player rotation: {playerRotation}  Angle limit: {360-angleLimit}");
        if (playerRotation > angleLimit && playerRotation < 360-angleLimit)
        {
            
            Debug.Log("<Color=Orange>LOSING LIFE</Color>");
            _lostBalance += Time.deltaTime;
        }

        if (_lostBalance >= timeToSubstractLife)
        {
            _lostBalance = 0;
            if(!StateMachine.GodMode) StateMachine.SubtractLife(1, this);
        }
    }

    public override void Exit()
    {
        base.Exit();
        Quaternion rot = _moveTarget.rotation;
        rot.z = 0;
        rot.x = 0;
        _moveTarget.rotation = rot;
    }

    private void OnCollision(Collider info)
    {
        if (!Active || _staggered) return;

        if (!info.gameObject.CompareTag("Grind"))
        {
            if(Active)
                StateMachine.SwitchState(StateMachine.GetState<PlayerMoveRunningState>());
            StateMachine.GetState<PlayerMoveRunningState>().DisableInvincibility();
        }
    }
    
}
