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

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

    }

    protected override void Update()
    {
        base.Update();

        AttackController();
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
        if (Time.time - lastAttack > cooldown && GameManager.instance.player.canMove)
        {

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {

                audioOnUse.Play();
                lastAttack = Time.time;
                Swing("SwingUp");

            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {

                audioOnUse.Play();
                lastAttack = Time.time;
                Swing("SwingDown");

            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {

                audioOnUse.Play();
                lastAttack = Time.time;

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

                audioOnUse.Play();
                lastAttack = Time.time;

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
        setAnimatorBool("BattleMode", true);
        GameManager.instance.player.Swing(triggerName); // Trigger Mover and hands animators

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
