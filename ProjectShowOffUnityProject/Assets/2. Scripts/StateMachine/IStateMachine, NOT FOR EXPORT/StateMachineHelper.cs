using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StateMachineHelper
{

    public static void SwitchState<StateType> (this IStateMachine<StateType> m, StateType newState) where StateType : IState
    {
        if (EqualityComparer<IState>.Default.Equals(m.CurrentState, newState))
        {
            //If current state is the same as the new state, return.
            if (!m.States.Contains(newState))
            {
                m.States.Add(newState);
            }
            m.CurrentState.Exit();
            m.CurrentState = newState;
            m.CurrentState.Enter();
            ;
            return;
        }
    }
    
    public static IState GetState<StateType, T>(this IStateMachine<StateType> m) where StateType : IState where T : StateType
    {   
        Debug.Log($"Getting Type: {typeof(T).Name}" );
        for (int i = 0; i < m.States.Count; i++)
        {
            if (typeof(T) == m.States[i].GetType())
            {
                Debug.Log("Found type!");
                return m.States[i];
            }
        }
        return default;
    }

}
