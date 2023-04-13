using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyState
{
    //For all states to inherit
    // The state manager can guarantee that all states have these functions

    public void Enter(EnemyStateMachine stateMachine, Enemy enemy);
    public void Execute(EnemyStateMachine stateMachine, Enemy enemy);
    public void Exit(EnemyStateMachine stateMachine, Enemy enemy);

}
