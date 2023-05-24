using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayState : GameState
{
    public override void Run()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _stateMachine.SwitchState(_stateMachine.GetState<PauseState>());
        }
    }

}
