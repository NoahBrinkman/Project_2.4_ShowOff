using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityState : PlayerMoveRunningState
{
    [Header("Invincibility")]
    [SerializeField] private float _duration;
    [SerializeField] private List<SkinnedMeshRenderer> _meshRenderer;
    [SerializeField] private float flickerTime = .25f;
    private float _flickerTimer;
    private float timer;
    public PlayerState ReturnTo;
    protected override void OnCollision(Collider info)
    {
       // base.OnCollision(info);
       if (!Active ) return;
       if (info.gameObject.CompareTag("Grind"))
       {
           if(Active)
               StateMachine.SwitchState(StateMachine.GetState<PlayerGrindingState>());
       }

       if (info.GetComponent<SwirlingPortal>() is SwirlingPortal p)
       {
           p.Initialize();
           Teleport(p.TeleportPosition, p.OutwardDirection, p.targetBiome);
       }
    }

    public override void Enter()
    {
        base.Enter();
        _moving = true;
        // _meshRenderer.ForEach(x => x.enabled = false);
        _flickerTimer = 0;
        timer = 0;
    }

    public override void Run()
    {
        base.Run();
        _flickerTimer += Time.deltaTime;
        timer += Time.deltaTime;
        if (_flickerTimer >= flickerTime)
        {
           // _meshRenderer.ForEach(x => x.enabled = !x.enabled);
            _flickerTimer = 0;
        }

        if (timer >= _duration)
        {
            StateMachine.PathTracker.SnapMovement = false;
            if (ReturnTo.GetType() == typeof(PlayerMoveRunningState))
            {
                PlayerMoveRunningState p = (PlayerMoveRunningState)ReturnTo;
                p.SetTotalTimes(totalMoveTime,totalRotationTime);
            }
            StateMachine.SwitchState(ReturnTo);
        }
    }

    public override void Exit()
    {
        base.Exit();
     //   _meshRenderer.ForEach(x => enabled = true);
        _flickerTimer = 0;
        timer = 0;
    }
}
