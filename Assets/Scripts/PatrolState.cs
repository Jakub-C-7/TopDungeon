using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState
{
    Transform playerTransform;
    List<Vector3> patrolPoints;
    IPatrolEnemy patrolEnemy;

    int nextPatrolIndex;

    public void Enter(EnemyStateMachine stateMachine, Enemy enemy)
    {
        Debug.Log("I am patrolling");
        nextPatrolIndex = 1;

        playerTransform = GameObject.Find("Player").transform;
        enemy.healthBar.SetActive(false);
        patrolEnemy = enemy as IPatrolEnemy;
        if (patrolEnemy == null)
        {
            throw new System.Exception("Enemy assigned SplitEnemyState does not implement IPatrolEnemy");
        }
        else
        {
            patrolPoints = patrolEnemy.GetPatrolPoints();
        }
    }

    public void Execute(EnemyStateMachine stateMachine, Enemy enemy)
    {
        if (Vector3.Distance(playerTransform.position, enemy.startingPosition) < enemy.chaseLength)
        {
            if (Vector3.Distance(playerTransform.position, enemy.startingPosition) < enemy.triggerLength)
            {
                stateMachine.ChangeState(stateMachine.stateMapper[EnemyStatePhases.Pathing]);

            }
            else
            {
                Patrol(stateMachine, enemy);
            }

        }
        else // The player is out of range
        {
            Patrol(stateMachine, enemy);
        }
    }

    public void Exit()
    {
    }

    private void Patrol(EnemyStateMachine stateMachine, Enemy enemy)
    {
        enemy.UpdateMotor(patrolPoints[nextPatrolIndex] - enemy.transform.position);
        if (enemy.transform.position.x > patrolPoints[nextPatrolIndex].x - 0.01f && enemy.transform.position.x < patrolPoints[nextPatrolIndex].x + 0.01f)
        {
            if (enemy.transform.position.y > patrolPoints[nextPatrolIndex].y - 0.01f && enemy.transform.position.y < patrolPoints[nextPatrolIndex].y + 0.01f)
            {
                nextPatrolIndex = (nextPatrolIndex + 1) % patrolPoints.Count;
            }
        }


    }


}
