using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSmallEnemy : Enemy
{

    public float attackCooldown = 1.5f;
    public float lastAttack;
    GameObject projectile;
    Transform playerTransform;
    public int damageAmount;
    public float pushForce;
    public float range;
    protected override void Start()
    {
        base.Start();
        projectile = GameManager.instance.prefabList.Find(x => x.name.Equals("ice_shard"));
        Projectile projectileComponent = projectile.GetComponent<Projectile>();
        projectileComponent.damageAmount = damageAmount;
        projectileComponent.pushForce = pushForce;
        projectileComponent.origin = transform.name;
        lastAttack = Time.time - attackCooldown;
        playerTransform = GameObject.Find("Player").transform;
      
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
            lastAttack = Time.time;
            GameObject projectileInstance = Instantiate(projectile, transform.position, Quaternion.identity);
            projectileInstance.GetComponent<Rigidbody2D>().velocity = (playerTransform.position - transform.position).normalized;
            Vector3 velocity = projectileInstance.GetComponent<Rigidbody2D>().velocity;
            projectileInstance.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg);
            Destroy(projectileInstance, range);

        }
    }
   
}
