using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    //For all states to inherit
    // The state manager can guarantee that all states have these functions

    public void Enter(StateMachine stateMachine, Enemy enemy);
    public void Execute(StateMachine stateMachine, Enemy enemy);
    public void Exit();

}
