


using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
   public State CurrentState { get; private set; }
   
   private List<State> states = new List<State>();

   private void Start()
   {
      foreach (Transform t in transform)
      {
         State s = t.GetComponent<State>();
         if (s != null)
         {
            states.Add(s);
            s.SetStateMachine(this);
         }
      }
      CurrentState = states[0];
      CurrentState.OnStateEntered();
   }

   public void SwitchState(State newState)
   {
      if(CurrentState == newState) return;
    //  Debug.Log($"Exitting {CurrentState.name} and entering {newState.name}");
      CurrentState.OnStateExit();
      CurrentState = newState;
      CurrentState.OnStateEntered();
   }


   public State GetState<T>() where T : State
   {
      State s = null;
      Debug.Log($"Getting Type: {typeof(T).Name}" );
      for (int i = 0; i < states.Count; i++)
      {

      
         if (typeof(T) == states[i].GetType())
         {
            Debug.Log("Found type!");
            return states[i];
         }
      }

      return s;
   }
   
   private void Update()
   {
      CurrentState.Update();
   }
}
