using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseState : State
{
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _stateMachine.SwitchState(_stateMachine.GetState<PlayState>());
        }
    }
}
