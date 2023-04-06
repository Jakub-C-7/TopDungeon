using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSmallEnemy : Enemy, IProjectileEnemy
{
    Transform playerTransform;
    public float attackCooldown = 1.5f;
    public float lastAttack;
    public GameObject projectile;
    public int damageAmount;
    public float pushForce;
    public float projectileRange = 1f;

    public float GetLastAttack()
    {
        return lastAttack;
    }

    public Damage GetProjectileDamageObject()
    {
        return new Damage{
            origin = transform.position,
            damageAmount = damageAmount,
            pushForce = pushForce
        };
    }

    public GameObject GetProjectilePreFab()
    {
        return projectile;
    }

    public float GetRange()
    {
        return projectileRange;
    }

    public void SetLastAttack(float time)
    {
        lastAttack = time;
    }
     public float GetAttackCooldown()
    {
        return attackCooldown;
    }



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
