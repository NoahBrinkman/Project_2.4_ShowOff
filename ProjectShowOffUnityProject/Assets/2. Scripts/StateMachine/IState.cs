using UnityEngine.Events;

public interface IState
{
  
    public void Enter();
    public void Run();
    public void Exit();
    
    public bool IsActive();
    public void AddToStateEnter(UnityAction pcall);
    public void AddToStateExit(UnityAction pcall);
    
    public void RemoveFromStateEnter(UnityAction pcall);
    public void RemoveFromStateExit(UnityAction pcall);
}
