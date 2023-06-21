using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlayerStateMachine : StateMachine<PlayerState>
    {
        [SerializeField] private List<PlayerState> _playerStates;
        [SerializeField] private float _scoreModifier = 1.2f;
        public GameObject CurrentRoad;
        
        [Header("Live & Death")]
        [SerializeField] private int _maxLives;

        [SerializeField, FormerlySerializedAs("OnDiePeasantDie")] private UnityEvent _onPlayerDeath;
        private int _lives;

        [HideInInspector] public int Lives => _lives;
        [HideInInspector] public int MaxLives => _maxLives;
        
        [field: Header("Roads")]
        [SerializeField] private List<RoadGenerator> biomes;
        [HideInInspector]
        public RoadGenerator ActiveRoad { get; set; }
        public PathTracker PathTracker { get; private set; }
        [HideInInspector] public bool GodMode = false;

        private void Awake()
        {
            PathTracker=  GetComponentInChildren<PathTracker>();
            _states = new List<PlayerState>(_playerStates);
            for (int i = 0; i < _states.Count; i++)
            {
                _states[i].StateMachine = this;
            }

            _lives = _maxLives;

        }

        private void Start()
        {
            if (biomes.Count > 0)
            {
                ActiveRoad = biomes[0];
                ActiveRoad.IsActive = true;
            }
            CurrentState = _states[0];
            CurrentState.Enter();
        }

        private void Update()
        {
            CurrentState.Run();
        }

        public void SubtractLife(int amount)
        {
            _lives -= amount;
            if (_lives <= 0)
            {
                _onPlayerDeath?.Invoke();
            }
        }

        public void Revive()
        {
            _lives = _maxLives;
            SwitchState(GetState<PlayerMoveRunningState>());
        }
        public void BackToMoveOrDeathState()
        {
            if (_lives > 0)
            {
                SwitchState(GetState<PlayerMoveRunningState>());
            }
            else
            {
                SwitchState(GetState<DyingState>());
            }
        }
        
        public List<RoadGenerator> GetBiomes()
        {
            return biomes;
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
