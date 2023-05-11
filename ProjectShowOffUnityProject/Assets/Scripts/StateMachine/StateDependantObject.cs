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

public class StateDependantObject : MonoBehaviour
{
    [SerializeField] protected State state;
    [SerializeField] private InactiveAction _whatToDoWhenInactive = InactiveAction.DoNotUpdate;


    protected virtual void Start()
    {
        if (_whatToDoWhenInactive == InactiveAction.Disable)
        {
            state.AddListenerOnStateEnter(
                delegate
                {
                    gameObject.SetActive(true);
                });
            state.AddListenerOnStateExit(
                delegate
                {
                    gameObject.SetActive(false);
                });
        }
    }

    private void Update()
    {
        if (!state.Active && _whatToDoWhenInactive != InactiveAction.UpdateRegardless)
        {
            if (_whatToDoWhenInactive == InactiveAction.DoNotUpdate)
            {
                return;
            }
        }
        ReNew();
    }

    protected virtual void ReNew()
    {
        
    }
    
}
