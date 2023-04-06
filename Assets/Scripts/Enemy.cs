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
    private BoxCollider2D hitBox;
   // public Collider2D[] hits = new Collider2D[10];

    protected EnemyStateMachine stateMachine;
    
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

    protected bool dead = false;
    protected float fade = 1;

    protected override void Start()
    {
        base.Start();
        stateMachine = new EnemyStateMachine(this);
       
        startingPosition = transform.position;
        
         

        stateMachine.stateMapper = new Dictionary<EnemyStatePhases, IEnemyState>{
            [EnemyStatePhases.Idle] = new IdleState(),
            [EnemyStatePhases.Pathing] = new ChaseState()
        };
        stateMachine.ChangeState(stateMachine.stateMapper[EnemyStatePhases.Idle]);

    }

    protected override void ReceiveDamage(Damage dmg)
    {
        base.ReceiveDamage(dmg);
        OnHealthChange();
        GameManager.instance.StartCoroutine(SetGlitch());
            
    }

    IEnumerator SetGlitch(){
        if(this){
            this.GetComponent<SpriteRenderer>().material.SetFloat("_Glitch", 0.9f);
            yield return new WaitForSeconds(0.2f);
        }
        if(this){
            this.GetComponent<SpriteRenderer>().material.SetFloat("_Glitch",0f);
        }
    }


        // Stagger(); // initiate Stagger
        // Invoke("RecoverFromStagger", 2f); // Initiate recovery from stagger
    

    private void OnHealthChange()
    {
        float ratio = (float)this.hitPoints / (float)this.maxHitpoints;
        health.fillAmount = ratio;
    }
    protected override void Death()
    {
        dead = true;
        Destroy(hitBox);
      
    }

    protected void CleanUpDeath(){
        Destroy(gameObject);
        GameManager.instance.GrantXp(xpValue);
        GameManager.instance.RegisterDeath(this.gameObject.GetComponent<SpriteRenderer>().sprite);
        GameManager.instance.ShowText("+ " + xpValue + " xp", 30, Color.magenta, transform.position, Vector3.up * 40, 1.0f);
    }


    protected void FixedUpdate(){
        if(!dead){
            Execute();
        }else{
            fade -= Time.deltaTime;
            if(fade <= 0f){
                CleanUpDeath();
            }
            this.GetComponent<SpriteRenderer>().material.SetFloat("_Fade", fade);

        }
    }

    protected virtual void Execute(){
       stateMachine.Update(); 
    }

    

  

}
