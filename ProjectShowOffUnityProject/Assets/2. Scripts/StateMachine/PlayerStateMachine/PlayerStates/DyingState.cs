using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DyingState : PlayerState
{
    
    [SerializeField] private bool enableReviving = true;
    [SerializeField] private int reviveTime = 30;
    [SerializeField] private Slider slider;
    [SerializeField] private KeyCode leftKey = KeyCode.Z;
    [SerializeField] private KeyCode rightKey = KeyCode.X;
    private int _value;
    [HideInInspector] public PlayerState ReturnToState;

    public override void Enter()
    {
        base.Enter();
        _value = 0;
        enableReviving = true;
    }

    public override void Run()
    {
        base.Run();
        if (enableReviving)
        {
            if (Input.GetKeyUp(leftKey) && _value % 2 == 0)
            {
                _value++;

            } else if (Input.GetKeyUp(rightKey) && _value % 2 == 1)
            {
                _value++;
            }
        }

        slider.value = _value;
        if (_value >= reviveTime)
        {
            Debug.LogWarning("YOU REVIVED!!!");
            enableReviving = false;
                StateMachine.SubtractLife(-StateMachine.MaxLives);
            if (ReturnToState != null)
            {
                InvincibilityState i = (InvincibilityState)StateMachine.GetState<InvincibilityState>();
                i.ReturnTo = ReturnToState;
                StateMachine.SwitchState(i);
            }
            else
            {
                StateMachine.SwitchState(StateMachine.GetState<PlayerMoveRunningState>());
            }
        }
    }

}
