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


    [SerializeField] 
    private string _horizontalAxis = "Horizontal";

    [Header("Grinding")]
    [SerializeField] private float angleOffset = 5;
    [SerializeField] private float angleLimit = 30;
    private bool _staggered = false;
    private float _angle;
    
    private void Start()
    {
        _col.OnCollisionEnterEvent.AddListener(OnCollision);
    }


    public override void Enter()
    {
        base.Enter();
        Debug.Log("I AM GRINDING YESSS");
        _staggered = false;
        _moving = true;
    }


    public override void Run()
    {
        base.Run();
        Balance();
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
    
}
