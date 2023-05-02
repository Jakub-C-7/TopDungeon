using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Collidable
{
    // Damage structure
    public int[] damageAmount = { 1, 2, 3, 4, 5, 6, 7 };
    public float[] pushForce = { 2.0f, 2.2f, 2.5f, 3f, 3.2f, 3.6f, 4f };
    // Weapon Upgrade
    public int weaponLevel = 0;
    public SpriteRenderer spriteRenderer;
    // Visual Feedback
    public Animator animator;
    public AudioSource audioOnUse;
    // Weapon Attacks
    public float cooldown = 0.5f;
    protected float lastAttack;

    private Dictionary<int, float> lastHitEnemyDict;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        lastHitEnemyDict = new Dictionary<int, float>();

    }

    protected override void Update()
    {
        base.Update();

        AttackController();

        //Find enemies that's cooldown period has run out
        List<int> candidatesForRemoval = new List<int>();
        foreach (var (enemyID, timeOfLastHit) in lastHitEnemyDict)
        {
            if (Time.time > (timeOfLastHit + cooldown))
            {
                candidatesForRemoval.Add(enemyID);
            }
        }
        // Remove all enemies that's cooldown periods have run out
        foreach (int candidate in candidatesForRemoval)
        {
            lastHitEnemyDict.Remove(candidate);
        }

    }

    // Do damage to enemies with the sword
    protected override void OnCollide(Collider2D coll)
    {
        if (coll.tag == "Fighter")
        {
            if (coll.name == "Player")
            {
                return;
            }
            //Check if currently in cooldown period, otherwise add new entry to cooldown
            int collId = coll.GetInstanceID();
            if (lastHitEnemyDict.ContainsKey(collId))
            {
                return;
            }
            else
            {
                lastHitEnemyDict[collId] = Time.time;
            }
            //Create new damage object, send it to the fighter that we've hit
            Damage damage = new Damage
            {
                damageAmount = damageAmount[weaponLevel],
                origin = transform.position,
                pushForce = pushForce[weaponLevel]

            };

            coll.SendMessage("ReceiveDamage", damage);
        }
    }

    protected virtual void AttackController()
    {
        // if (Time.time - lastAttack > cooldown && GameManager.instance.player.canMove)
        // {

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (Time.time - lastAttack > cooldown && GameManager.instance.player.canMove)
            {
                audioOnUse.Play();
                lastAttack = Time.time;
            }
            ToggleCombo(true);

            Swing("SwingUp");

        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {

            if (Time.time - lastAttack > cooldown && GameManager.instance.player.canMove)
            {
                audioOnUse.Play();
                lastAttack = Time.time;
            }
            ToggleCombo(true);

            Swing("SwingDown");

        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (Time.time - lastAttack > cooldown && GameManager.instance.player.canMove)
            {
                audioOnUse.Play();
                lastAttack = Time.time;
            }
            ToggleCombo(true);

            if (PlayerDirectionX() == "right")
            {

                Swing("SwingForward");

            }
            else
            {

                Swing("SwingBackward");

            }

        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {

            if (Time.time - lastAttack > cooldown && GameManager.instance.player.canMove)
            {
                audioOnUse.Play();
                lastAttack = Time.time;
            }
            ToggleCombo(true);

            if (PlayerDirectionX() == "left")
            {

                Swing("SwingForward");

            }
            else
            {

                Swing("SwingBackward");

            }

        }
    }

    private string PlayerDirectionX()
    {
        if (GameManager.instance.player.transform.localScale.x > 0)
        {
            return "right";
        }
        else if (GameManager.instance.player.transform.localScale.x < 0)
        {
            return "left";
        }
        else
        {
            return "none";
        }

    }

    private void Swing(string triggerName)
    {
        animator.SetTrigger(triggerName);
        GameManager.instance.player.Swing(triggerName); // Trigger Mover and hands animators

    }

    private void ToggleCombo(bool status)
    {
        // setAnimatorBool("InCombo", status);
        GameManager.instance.player.ToggleCombo(status); // Trigger Mover and hands animators

    }

    public void setAnimatorBool(string name, bool value)
    {
        animator.SetBool(name, value);
    }

    // Upgrade player's weapon level
    public void UpgradeWeapon()
    {
        weaponLevel++;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];

        //Change stats of weapon
    }

    public void SetWeaponLevel(int level)
    {
        weaponLevel = level;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];
    }

    public void SetWeaponImage(Sprite sprite)
    {
        spriteRenderer.sprite = Sprite.Create(sprite.texture, sprite.textureRect, new Vector2(0.5f, 0.5f));
    }

}
