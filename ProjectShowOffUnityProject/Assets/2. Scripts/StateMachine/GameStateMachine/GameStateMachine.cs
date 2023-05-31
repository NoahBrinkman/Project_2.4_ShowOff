


using System;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine : StateMachine<GameState>
{
    private void Start()
    {
        foreach (Transform t in transform)
        {
            GameState s = t.GetComponent<GameState>();
            if (s != null)
            {
                _states.Add(s);
                s.SetStateMachine(this);
            }
        }
        CurrentState = _states[0];
        CurrentState.Enter();
    }

}
