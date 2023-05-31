using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class GameState : MonoBehaviour, IState
{
    protected GameStateMachine _stateMachine;
    public bool Active { get; protected set; }

    [SerializeField] private UnityEvent _onStateEnteredEvent;

    public virtual void Enter()
    {
        Active = true;
        _onStateEnteredEvent?.Invoke();
    }

    public virtual void Run()
    {
       
    }


    [SerializeField] private UnityEvent _onStateExitEvent;

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

