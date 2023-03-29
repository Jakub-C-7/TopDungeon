using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Mover
{
    // Experience
    public int xpValue = 1;

    // Logic
    public float triggerLength = 0.5f; //Aggro ranges
    public float chaseLength = 1.5f;
    private bool chasing;
    public bool collidingWithPlayer;
    public Vector3 startingPosition;
    public Image health;
    public GameObject healthBar;
    // Hitbox
    public ContactFilter2D filter;
    public BoxCollider2D hitBox;
   // public Collider2D[] hits = new Collider2D[10];

    protected StateMachine stateMachine;
    
    public ParticleSystem explosionParticleSystem;
    
    // used by projectile launching enemeies
    public float attackCooldown = 1.5f;
    public float lastAttack;
    public GameObject projectile;
    public int damageAmount;
    public float pushForce;
    public float range;

    public int round =0;
    public int maxround= 4;

    protected override void Start()
    {
        base.Start();
        stateMachine = new StateMachine(this);
       
        startingPosition = transform.position;
        
         

        stateMachine.stateMapper = new Dictionary<EnemyStatePhases, IState>{
            [EnemyStatePhases.Idle] = new IdleState(),
            [EnemyStatePhases.Pathing] = new ChaseState()
        };
        stateMachine.ChangeState(stateMachine.stateMapper[EnemyStatePhases.Idle]);

    }

    protected override void ReceiveDamage(Damage dmg)
    {
        base.ReceiveDamage(dmg);
        OnHealthChange();

        // Stagger(); // initiate Stagger
        // Invoke("RecoverFromStagger", 2f); // Initiate recovery from stagger
    }

    private void OnHealthChange()
    {
        float ratio = (float)this.hitPoints / (float)this.maxHitpoints;
        health.fillAmount = ratio;
    }
    protected override void Death()
    {
        Destroy(gameObject);
        GameManager.instance.GrantXp(xpValue);
        GameManager.instance.RegisterDeath(this.gameObject.GetComponent<SpriteRenderer>().sprite);
        GameManager.instance.ShowText("+ " + xpValue + " xp", 30, Color.magenta, transform.position, Vector3.up * 40, 1.0f);
    }

    protected void FixedUpdate(){
        stateMachine.Update();
    }

  

}
