using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PlayState : GameState
{
    [SerializeField] private List<PlayerStateMachine> _players;
    public UnityEvent OnGameOver;
    public override void Run()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _stateMachine.SwitchState(_stateMachine.GetState<PauseState>());
        }
    }

    public void CheckIfAllPlayersDied()
    {
        if (_players.All(p => p.Lives <= 0))
        {
            OnGameOver?.Invoke();
        }
    }
}
