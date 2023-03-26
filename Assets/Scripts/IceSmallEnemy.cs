using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSmallEnemy : Enemy
{

    public float attackCooldown = 1.5f;
    public float lastAttack;
    protected override void Start()
    {
        base.Start();
        lastAttack = Time.time - attackCooldown;
      
        stateMachine.stateMapper = new Dictionary<EnemyStatePhases, IState>{
            [EnemyStatePhases.Idle] = new IdleState(),
            [EnemyStatePhases.Pathing] = new MaintainDistanceState(),
            [EnemyStatePhases.Fighting] = new LaunchProjectileState()
        };
    }

     new protected void FixedUpdate(){
    
      
        
        stateMachine.Update();
        
        
    }

    public override void LaunchProjectile(){
        if(Time.time - lastAttack > attackCooldown){
            Debug.Log("Launch projectile");
            lastAttack = Time.time;

        }
    }
   
}
