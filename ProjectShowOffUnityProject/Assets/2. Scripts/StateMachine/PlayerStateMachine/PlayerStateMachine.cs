using System;
using System.Collections.Generic;
using UnityEngine;

    public class PlayerStateMachine : StateMachine<PlayerState>
    {
        [SerializeField] private List<PlayerState> _playerStates;
       
       private void Start()
       {
            _states = new List<PlayerState>(_playerStates);
            CurrentState = _playerStates[0];
            CurrentState.Enter();
            
        }

        private void Update()
        {
            CurrentState.Run();
        }
        

    }
