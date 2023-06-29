using System;
using System.Collections.Generic;
using UnityEngine;

    public class StateMachine<StateType> : MonoBehaviour where StateType : IState
    {

        public StateType CurrentState { get; protected set; }
        
        protected List<StateType> _states = new List<StateType>();
        
        
        /// <summary>
        /// Fint
        /// </summary>
        /// <param name="newPlayState"></param>
        public void SwitchState(StateType newPlayState)
        {
            if (EqualityComparer<StateType>.Default.Equals(CurrentState, newPlayState))
            {
                //If current state is the same as the new state, return.
                Debug.LogError("ËRROR: SAME STATE");
                return;
            }

            try
            {
                CurrentState.Exit();
                CurrentState = newPlayState;
                CurrentState.Enter();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        /// <summary>
        /// returns a reference to the state you set in type parameter T.
        /// if it cannot find anything return default or null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>

        public T GetState<T>() where T : StateType
        {
          
//            Debug.Log($"Getting Type: {typeof(T).Name}" );
            for (int i = 0; i < _states.Count; i++)
            {
                if (typeof(T) == _states[i].GetType())
                {
//                    Debug.Log("Found type!");
                    return (T)_states[i];
                }
            }

            return default;
        }
        
        /// <summary>
        /// Get the specific type of a state
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public StateType FindState<T>() where T : StateType
        {
          
            Debug.Log($"Getting Type: {typeof(T).Name}" );
            for (int i = 0; i < _states.Count; i++)
            {
                if (typeof(T) == _states[i].GetType())
                {
                    Debug.Log("Found type!");
                    return _states[i];
                }
            }

            return default;
        }
        
        public IState GetCurrentState()
        {
            return CurrentState;
        }
        
        private void Update()
        {
            CurrentState.Run();
        }

        
    }