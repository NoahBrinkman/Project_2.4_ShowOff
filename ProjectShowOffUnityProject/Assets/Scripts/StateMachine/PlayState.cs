using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayState : State
{
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _stateMachine.SwitchState(_stateMachine.GetState<PauseState>());
        }
    }

}
