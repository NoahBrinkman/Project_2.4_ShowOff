﻿using System;
using System.Collections.Generic;
using UnityEngine;

    public class PlayerStateMachine : StateMachine<PlayerState>
    {
        [SerializeField] private List<PlayerState> _playerStates;
        [SerializeField] private float _scoreModifier = 1.2f;
        public GameObject CurrentRoad;
        
        [field: Header("Roads")]
        [SerializeField] private List<RoadGenerator> biomes;
        [HideInInspector]
        public RoadGenerator ActiveRoad { get; private set; }
        public PathTracker PathTracker { get; private set; }
        
        private void Awake()
        {
            PathTracker=  GetComponentInChildren<PathTracker>();
            _states = new List<PlayerState>(_playerStates);
            for (int i = 0; i < _states.Count; i++)
            {
                _states[i].StateMachine = this;
            }

            
        }

        private void Start()
        {
            ScoreManager.Instance.AddPlayer(this);
            if (biomes.Count > 0)
            {
                ActiveRoad = biomes[0];
                ActiveRoad.IsActive = true;
            }
            CurrentState = _playerStates[0];
            CurrentState.Enter();
        }

        private void Update()
        {
            CurrentState.Run();
        }


        public float GetScore() 
        {
            if (CurrentState.GetType() != typeof(PlayerStaggerState))
            {
                return 1 * _scoreModifier;
            }
            else
            {
                return 0;
            }
        }
    }
