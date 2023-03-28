using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchProjectileState : IState
{
    public void Enter(StateMachine stateMachine, Enemy enemy)
    {
    }

    public void Execute(StateMachine stateMachine, Enemy enemy)
    {
        enemy.LaunchProjectile();
        stateMachine.ChangeState(stateMachine.stateMapper[EnemyStatePhases.Pathing]);
    }

    public void Exit()
    {
    }

  
}
