using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatusState
{
    //For all states to inherit
    // The state manager can guarantee that all states have these functions
    public void Enter(StatusStateMachine stateMachine, Mover mover, float duration);
    public void Execute(StatusStateMachine stateMachine, Mover mover);
    public void Exit();
    public string GetId();

}