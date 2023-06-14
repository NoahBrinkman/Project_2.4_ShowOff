using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrindingState : PlayerOnTrackState
{
    
    /*You have access to the playerstatemachine and also the target points, passed points etc
     You can alter any input in here. Check the PlayerOnTrackState to see what you have (you should have most things required to work)
     */
    
    /// <summary>
    /// When the player enters the state
    /// </summary>
    public override void Enter()
    {
        base.Enter();
    }
    
    /// <summary>
    /// When the player exits the state
    /// </summary>
    public override void Exit()
    {
        base.Exit();
    }

    /// <summary>
    /// Every frame when the state is active
    /// </summary>
    public override void Run()
    {
        base.Run();
    }
    
}
