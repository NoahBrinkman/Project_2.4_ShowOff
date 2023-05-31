using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum InactiveAction
{
    DoNotUpdate,
    UpdateRegardless,
    Disable,
    
}

public class StateDependantObject<StateType> : MonoBehaviour where StateType : IState  
{
    [Header("StateLogic")]
    [SerializeField] protected StateType _state;
    [SerializeField] private InactiveAction _whatToDoWhenInactive = InactiveAction.DoNotUpdate;


    protected virtual void Start()
    {
        if (_whatToDoWhenInactive == InactiveAction.Disable)
        {
            _state.AddToStateEnter(delegate() { gameObject.SetActive(true); });
            _state.AddToStateExit(delegate() { gameObject.SetActive(false); });
            if (!_state.IsActive())
            {
                gameObject.SetActive(false);
            }
        }   
    }

    private void Update()
    {
        if (!_state.IsActive() && _whatToDoWhenInactive == InactiveAction.DoNotUpdate)
        {
            return;
        }
        Run();
    }

    protected virtual void Run()
    {
        
    }
    
}
