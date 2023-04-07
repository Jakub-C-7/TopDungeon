using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSmallEnemy : Enemy, IWaveProjectileEnemy
{    
    Transform playerTransform;
    public float attackCooldown = 1.5f;
    public float lastAttack;
    public GameObject projectile;
    public int damageAmount;
    public float pushForce;
    public float projectileRange;

    private int round = 0;
    public int maxRound = 4;


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

    public int GetRound()
    {
        return round;
    }

    public void SetRound(int round)
    {
        this.round = round;
        
    }

    public int GetMaxRound()
    {
        return maxRound;
    }

    protected override void Start()
    {
        
        base.Start();
        lastAttack = Time.time - attackCooldown;
        projectile = GameManager.instance.prefabList.Find(x => x.name.Equals("flame_projectile"));
        lastAttack = Time.time - attackCooldown;
        playerTransform = GameObject.Find("Player").transform;
    
      
        stateMachine.stateMapper = new Dictionary<EnemyStatePhases, IEnemyState>{
            [EnemyStatePhases.Idle] = new IdleState(),
            [EnemyStatePhases.Pathing] = new MaintainDistanceState(),
            [EnemyStatePhases.Fighting] = new LaunchWaveProjectileState()
        };
    }

    
}