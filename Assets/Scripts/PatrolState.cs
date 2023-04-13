using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState
{
    Transform playerTransform;
    List<Vector3> patrolPoints;
    List<PathNode> patrolWayPoints;
    IPatrolEnemy patrolEnemy;
    PathFinding pathFinding;
    int currentLocation;
    int nextLocation;

    int nextPatrolIndex;

    public void Enter(EnemyStateMachine stateMachine, Enemy enemy)
    {
        Debug.Log("I am patrolling");
        nextPatrolIndex = 1; //set the patrol index to mark the first waypoint 

        //use index set where the enemy currently is and where they are going in the list of locations
        currentLocation = 0;
        nextLocation = 1;

        playerTransform = GameObject.Find("Player").transform;
        enemy.healthBar.SetActive(false);
        patrolEnemy = enemy as IPatrolEnemy;
        if (patrolEnemy == null)
        {
            throw new System.Exception("Enemy assigned SplitEnemyState does not implement IPatrolEnemy");
        }
        else
        {
            pathFinding = new PathFinding();
            patrolPoints = patrolEnemy.GetPatrolPoints();
            CalculateWayPoints();
        }
    }

    public void Execute(EnemyStateMachine stateMachine, Enemy enemy)
    {
        if (Vector3.Distance(playerTransform.position, enemy.transform.position) < enemy.chaseLength)
        {
            if (Vector3.Distance(playerTransform.position, enemy.transform.position) < enemy.triggerLength)
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

    private void CalculateWayPoints()
    {
        patrolWayPoints = pathFinding.FindPath(patrolPoints[currentLocation].x, patrolPoints[currentLocation].y, patrolPoints[nextLocation].x, patrolPoints[nextLocation].y);
    }

    private void CalculateNextJourneyPoints()
    {
        nextPatrolIndex = 0;
        currentLocation = nextLocation;
        nextLocation = (nextLocation + 1) % patrolPoints.Count;
    }

    private void Patrol(EnemyStateMachine stateMachine, Enemy enemy)
    {


        if (patrolWayPoints == null)
        {
            CalculateNextJourneyPoints();
            CalculateWayPoints();
        }

        //If you've reached your destination waypoint, move to the next waypoint
        if (enemy.transform.position.x > patrolWayPoints[nextPatrolIndex].x - 0.16f && enemy.transform.position.x < patrolWayPoints[nextPatrolIndex].x + 0.16f)
        {
            if (enemy.transform.position.y > patrolWayPoints[nextPatrolIndex].y - 0.16f && enemy.transform.position.y < patrolWayPoints[nextPatrolIndex].y + 0.16f)
            {
                if (nextPatrolIndex + 1 < patrolWayPoints.Count)
                {
                    nextPatrolIndex = (nextPatrolIndex + 1);
                }
                else
                {
                    //if you've run out of waypoints in the current journey, calculate your next waypoints for your next journey
                    CalculateNextJourneyPoints();
                    CalculateWayPoints();

                }
            }
        }
        enemy.UpdateMotor((new Vector3(patrolWayPoints[nextPatrolIndex].x, patrolWayPoints[nextPatrolIndex].y, 0) - enemy.transform.position).normalized);



    }


}
