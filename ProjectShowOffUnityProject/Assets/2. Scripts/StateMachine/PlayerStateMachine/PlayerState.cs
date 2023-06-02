using UnityEngine;
using UnityEngine.Events;

public class PlayerState : MonoBehaviour, IState
    {   
        [Header("State")]
        [SerializeField] private UnityEvent _onStateEnteredEvent;
        [SerializeField] private UnityEvent _onStateExitEvent;
        protected GameStateMachine _stateMachine;
        public bool Active { get; protected set; }


        public virtual void Enter()
        {
            Active = true;
            _onStateEnteredEvent?.Invoke();
        }

        public virtual void Run()
        {
            
        }



        public virtual void Exit()
        {
            Active = false;
            _onStateExitEvent?.Invoke();
        }

        public bool IsActive()
        {
            return Active;
        }

        public void AddToStateEnter(UnityAction pcall)
        {
            _onStateEnteredEvent.AddListener(pcall);
       
        }

        public void AddToStateExit(UnityAction pcall)
        {
            _onStateExitEvent.AddListener(pcall);
        }

        public void RemoveFromStateEnter(UnityAction pcall)
        {
            _onStateEnteredEvent.RemoveListener(pcall);
        }

        public void RemoveFromStateExit(UnityAction pcall)
        {
            _onStateExitEvent.RemoveListener(pcall);
        }



        public void SetStateMachine(GameStateMachine pStateMachine)
        {
            _stateMachine = pStateMachine;
        }

       
    }
