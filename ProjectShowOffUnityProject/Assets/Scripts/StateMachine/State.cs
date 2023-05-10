using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class State : MonoBehaviour
{
    protected StateMachine _stateMachine;
    public bool Active { get; protected set; }

    [SerializeField] private UnityEvent _onStateEnteredEvent;

    public virtual void OnStateEntered()
    {
        Active = true;
        _onStateEnteredEvent?.Invoke();
    }

    public abstract void Update();
    
    [SerializeField] private UnityEvent _onStatExitEvent;

    public virtual void OnStateExit()
    {
        Active = false;
        _onStatExitEvent?.Invoke();
    }
    
    
    public void SetStateMachine(StateMachine pStateMachine)
    {
        _stateMachine = pStateMachine;
    } 
}

