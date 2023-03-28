using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSmallEnemy : Enemy
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
            int numberOfProjectiles = CalculateNumberOfProjectiles(projectile);
            float gap = (float)1/(float)numberOfProjectiles;

            for(int i = 0; i <CalculateNumberOfProjectiles(projectile); i++ ){
                Debug.Log("Projectile "+ i);
                GameObject projectileInstance = Instantiate(projectile, RandomCircle(gap * i, transform.position , 0.1f), Quaternion.identity); 
                // Ga ImeObject projectileInstance = Instantiate(projectile, RandomCircle(gap * i, transform.position , 0.1f), Quaternion.identity) as GameObject;
                Projectile projectileComponent = projectileInstance.AddComponent<Projectile>();
                projectileComponent.SetProjectileStats(damageAmount, pushForce, transform.name, "Player");
                projectileInstance.GetComponent<Rigidbody2D>().velocity = (projectileComponent.transform.position - transform.position).normalized;
                Vector3 velocity = projectileInstance.GetComponent<Rigidbody2D>().velocity;
                projectileInstance.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg);
                Destroy(projectileInstance, range);
            }
         

        }
    }


    private int CalculateNumberOfProjectiles(GameObject projectile){
        float radiusOfSprite = transform.GetComponent<SpriteRenderer>().sprite.rect.height / 2;
        float circumference = 2 * Mathf.PI * radiusOfSprite;
        float numberOfProjectiles = circumference / projectile.GetComponent<SpriteRenderer>().sprite.rect.height;
        return Mathf.FloorToInt(numberOfProjectiles);
    }

    private Vector3 RandomCircle(float value, Vector3 center, float radius){ 
    // create random angle between 0 to 360 degrees 
        Debug.Log(value);
        var ang = value * 360; 
        var pos = new Vector3();
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad); 
        pos.z = center.z; 
        Debug.Log(pos);
        return pos;
    }
}