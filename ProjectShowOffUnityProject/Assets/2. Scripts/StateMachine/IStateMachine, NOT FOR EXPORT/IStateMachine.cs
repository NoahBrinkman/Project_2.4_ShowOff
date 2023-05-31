using System.Collections.Generic;

public interface IStateMachine<StateType> where StateType : IState
{
    public StateType CurrentState { get; set; }
    public List<StateType> States { get; protected set; }
}
