using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSmallEnemy : Enemy
{


    protected override void Start()
    {
        base.Start();
        projectile = GameManager.instance.prefabList.Find(x => x.name.Equals("ice_shard"));
        lastAttack = Time.time - attackCooldown;      
        stateMachine.stateMapper = new Dictionary<EnemyStatePhases, IEnemyState>{
            [EnemyStatePhases.Idle] = new IdleState(),
            [EnemyStatePhases.Pathing] = new MaintainDistanceState(),
            [EnemyStatePhases.Fighting] = new LaunchProjectileState()
        };
    }

    //  new protected void FixedUpdate(){
    //      stateMachine.Update();        
    // }


    
   
}
