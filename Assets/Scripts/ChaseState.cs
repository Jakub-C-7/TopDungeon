using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IState
{
    Transform playerTransform;
    public ContactFilter2D filter;
    public Collider2D[] hits = new Collider2D[10];
    public void Enter(StateMachine stateMachine, Enemy enemy)
    {
        enemy.healthBar.SetActive(true);
        playerTransform = GameObject.Find("Player").transform;
    }

    public void Execute(StateMachine stateMachine, Enemy enemy)
    {

        enemy.boxCollider.OverlapCollider(enemy.filter, hits);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null)
            {
                continue;

            }

            if (hits[i].tag == "Fighter" && hits[i].name == "Player")
            {
                enemy.collidingWithPlayer = true;
            }

            //Cleaning up the array
        hits[i] = null;

        }
        enemy.distanceToPlayer = Vector3.Distance(playerTransform.position, enemy.startingPosition);

         
        
        if(!enemy.collidingWithPlayer){
            enemy.UpdateMotor((playerTransform.position - enemy.transform.position).normalized); // Run towards the player
        }
    
        if (enemy.distanceToPlayer > enemy.chaseLength)
        {
                stateMachine.ChangeState(new IdleState());
            
        }
        enemy.collidingWithPlayer = false;
        
    }

    public void Exit()
    {
        
    }
}
