using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class IdleState : IEnemyState
{
    Transform playerTransform;
    public void Enter(EnemyStateMachine stateMachine, Enemy enemy)
    {
        enemy.healthBar.SetActive(false);
        playerTransform = GameObject.Find("Player").transform;

    }

    public void Execute(EnemyStateMachine stateMachine, Enemy enemy)
    {
        if (Vector3.Distance(playerTransform.position, enemy.startingPosition) < enemy.chaseLength)
        {
            if (Vector3.Distance(playerTransform.position, enemy.startingPosition) < enemy.triggerLength)
            {
                stateMachine.ChangeState(stateMachine.stateMapper[EnemyStatePhases.Pathing]);

            }

        }
        //Check for overlaps

    }

    public void Exit()
    {
    }

    // Start is called before the first frame update


}
