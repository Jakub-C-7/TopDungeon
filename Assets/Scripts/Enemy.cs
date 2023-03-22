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
    private bool collidingWithPlayer;
    private Transform playerTransform;
    private Vector3 startingPosition;
    public Image health;
    public GameObject healthBar;
    // Hitbox
    public ContactFilter2D filter;
    private BoxCollider2D hitBox;
    private Collider2D[] hits = new Collider2D[10];

    protected override void Start()
    {
        base.Start();
        playerTransform = GameObject.Find("Player").transform;
        startingPosition = transform.position;
        hitBox = transform.GetChild(0).GetComponent<BoxCollider2D>();

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


    protected void FixedUpdate()
    {
        //Collision work
        boxCollider.OverlapCollider(filter, hits);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null)
            {
                continue;

            }

            if (hits[i].tag == "Fighter" && hits[i].name == "Player")
            {
                collidingWithPlayer = true;
            }

            //Cleaning up the array
            hits[i] = null;

        }




        // Is the player in range?
        if (Vector3.Distance(playerTransform.position, startingPosition) < chaseLength)
        {
            if (Vector3.Distance(playerTransform.position, startingPosition) < triggerLength)
            {
                if (canMove)
                {
                    chasing = true;

                }
                else
                {
                    chasing = false;
                }

                healthBar.SetActive(true);

            }

            if (chasing)
            {
                if (!collidingWithPlayer)
                {
                    UpdateMotor((playerTransform.position - transform.position).normalized); // Run towards the player
                }

            }
            else
            {
                UpdateMotor(startingPosition - transform.position); // Go back to starting point
            }

        }
        else // The player is out of range
        {
            healthBar.SetActive(false);
            UpdateMotor(startingPosition - transform.position); // Go back 
            chasing = false;

        }

        //Check for overlaps
        collidingWithPlayer = false;

    }


}
