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

    public virtual void Update()
    { }

    [SerializeField] private UnityEvent _onStateExitEvent;

    public virtual void OnStateExit()
    {
        Active = false;
        _onStateExitEvent?.Invoke();
    }
    
    
    public void SetStateMachine(StateMachine pStateMachine)
    {
        _stateMachine = pStateMachine;
    }

    public void AddListenerOnStateEnter(UnityAction pcall)
    {
        Debug.Log("Trying to add");
        _onStateEnteredEvent.AddListener(pcall);
    }
    public void AddListenerOnStateExit(UnityAction pcall)
    {
        _onStateExitEvent.AddListener(pcall);
    }
    public void RemoveListenerOnStateEnter(UnityAction pcall)
    {
        _onStateEnteredEvent.RemoveListener(pcall);
    }
    public void RemoveListenerOnStateExit(UnityAction pcall)
    {
        _onStateExitEvent.RemoveListener(pcall);
    }
}

