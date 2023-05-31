using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseState : GameState
{
    public override void Run()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _stateMachine.SwitchState(_stateMachine.GetState<PlayState>());
        }
    }
}
