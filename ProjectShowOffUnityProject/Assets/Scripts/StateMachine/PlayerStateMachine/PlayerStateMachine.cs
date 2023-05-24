using System;
using System.Collections.Generic;
using UnityEngine;

    public class PlayerStateMachine : StateMachine<PlayerState> 
    {
        [SerializeField] private List<PlayerState> _playerStates;
        private void Start()
        {
            _states = _playerStates;
            CurrentState = _states[0];
            CurrentState.Enter();
        }
    }