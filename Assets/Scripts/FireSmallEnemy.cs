using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSmallEnemy : Enemy
{

    
    Transform playerTransform;
    
    protected override void Start()
    {
        
        base.Start();
        attackCooldown = 3f;
        projectile = GameManager.instance.prefabList.Find(x => x.name.Equals("flame_projectile"));
        lastAttack = Time.time - attackCooldown;
        playerTransform = GameObject.Find("Player").transform;
      
        stateMachine.stateMapper = new Dictionary<EnemyStatePhases, IState>{
            [EnemyStatePhases.Idle] = new IdleState(),
            [EnemyStatePhases.Pathing] = new MaintainDistanceState(),
            [EnemyStatePhases.Fighting] = new LaunchWaveProjectileState()
        };
    }

     new protected void FixedUpdate(){   
        stateMachine.Update();
    }


}