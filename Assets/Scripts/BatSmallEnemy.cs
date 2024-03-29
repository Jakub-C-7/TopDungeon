using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatSmallEnemy : Enemy, IPatrolEnemy
{
    public List<Vector3> patrolPoints;
    public List<Vector3> GetPatrolPoints()
    {
        return patrolPoints;
    }

    public (float, float) GetPatrolSpeed()
    {
        return (xSpeed / 2f, ySpeed / 2f);
    }

    protected override void Start()
    {

        base.Start();
        patrolPoints = new List<Vector3>();
        patrolPoints.Add(new Vector3(startingPosition.x, startingPosition.y, 0f));
        patrolPoints.Add(new Vector3(1.2f, startingPosition.y, 0f));

        // patrolPoints.Add(new Vector3(startingPosition.x + 0.2f, startingPosition.y, 0f));
        patrolPoints.Add(new Vector3(-0.30f, 0.662f, 0f));


        stateMachine.stateMapper = new Dictionary<EnemyStatePhases, IEnemyState>
        {
            [EnemyStatePhases.Idle] = new PatrolState(),
            [EnemyStatePhases.Retreating] = new RetreatingState(),
            [EnemyStatePhases.Pathing] = new ChaseState()
        };
        stateMachine.ChangeState(stateMachine.stateMapper[EnemyStatePhases.Idle]);

    }
}
