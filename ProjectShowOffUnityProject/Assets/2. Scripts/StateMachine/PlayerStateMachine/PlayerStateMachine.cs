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
            Debug.LogError("GOOD MORNING USA");
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
            Debug.LogError("TODAY IS GONNA BE THE DAY");
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

        public void SubtractLife(int amount, PlayerState s = null)
        {
            _lives -= amount;
            
            if (_lives <= 0)
            {
                if (s != null)
                {
                    GetState<DyingState>().ReturnToState = s;
                }
                _onPlayerDeath?.Invoke();
            }
        }

        public void Revive(PlayerState state)
        {
            _lives = _maxLives;
            if (Vector3.Distance(transform.position, PathTracker.TargetPoints[0].position) > .1f)
            {
                transform.position = PathTracker.TargetPoints[0].position;
            }
            
            SwitchState(state);
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

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            Gizmos.color = Color.red;
            for (int i = 0; i < PathTracker.PassedPoints.Count; i++)
            {
                Gizmos.DrawSphere(PathTracker.PassedPoints[i],.5f);
            }
            Gizmos.color = Color.green;
            for (int i = 0; i < PathTracker.TargetPoints.Count; i++)
            {
                Gizmos.DrawSphere(PathTracker.TargetPoints[i].position,.5f);
            }
        }
    }
