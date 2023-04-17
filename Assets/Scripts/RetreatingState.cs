using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetreatingState : IEnemyState

{
    Transform playerTransform;
    List<PathNode> retreatWayPoints;
    int nextStepIndex;
    PathFinding pathFinding;
    public void Enter(EnemyStateMachine stateMachine, Enemy enemy)
    {
        playerTransform = GameObject.Find("Player").transform;
        enemy.healthBar.SetActive(false);
        pathFinding = new PathFinding();
        nextStepIndex = 0; //set the fist step on the path back to the starting position 
        retreatWayPoints = pathFinding.FindPath(enemy.transform.position.x, enemy.transform.position.y, enemy.startingPosition.x, enemy.startingPosition.y);

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
                if (retreatWayPoints == null) //IF the route back to their starting position is blocked, recalculate
                {
                    enemy.UpdateMotor((enemy.startingPosition - enemy.transform.position).normalized);
                }
                else
                {
                    enemy.UpdateMotor((new Vector3(retreatWayPoints[nextStepIndex].x, retreatWayPoints[nextStepIndex].y, 0) - enemy.transform.position).normalized);
                }
            }

        }
        else // The player is out of range
        {
            if (retreatWayPoints == null) //IF the route back to their starting position is blocked, recalculate
            {
                enemy.UpdateMotor((enemy.startingPosition - enemy.transform.position).normalized);

            }
            else
            {
                enemy.UpdateMotor((new Vector3(retreatWayPoints[nextStepIndex].x, retreatWayPoints[nextStepIndex].y, 0) - enemy.transform.position).normalized);
            }
        }
        if (retreatWayPoints == null)
        {
            retreatWayPoints = pathFinding.FindPath(enemy.transform.position.x, enemy.transform.position.y, enemy.startingPosition.x, enemy.startingPosition.y);
            return;
        }
        if (enemy.transform.position.x > retreatWayPoints[nextStepIndex].x - 0.08f && enemy.transform.position.x < retreatWayPoints[nextStepIndex].x + 0.08f)
        {
            if (enemy.transform.position.y > retreatWayPoints[nextStepIndex].y - 0.08f && enemy.transform.position.y < retreatWayPoints[nextStepIndex].y + 0.08f)
            {
                if (nextStepIndex + 1 < retreatWayPoints.Count)
                {
                    nextStepIndex++;

                }
                else
                {
                    stateMachine.ChangeState(stateMachine.stateMapper[EnemyStatePhases.Idle]);
                }
            }
        }


    }
    public void Exit(EnemyStateMachine stateMachine, Enemy enemy)
    {

    }

    // Start is called before the first frame update
}
