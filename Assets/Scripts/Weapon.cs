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
    private Animator animator;
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.time - lastAttack > cooldown)
            {
                audioOnUse.Play();
                lastAttack = Time.time;
                Swing();
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (Time.time - lastAttack > cooldown)
            {
                audioOnUse.Play();
                lastAttack = Time.time;
                Swing();
            }

        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (Time.time - lastAttack > cooldown)
            {
                audioOnUse.Play();
                lastAttack = Time.time;
                Swing();
            }

        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (Time.time - lastAttack > cooldown)
            {
                audioOnUse.Play();
                lastAttack = Time.time;
                Swing();
            }

        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (Time.time - lastAttack > cooldown)
            {
                audioOnUse.Play();
                lastAttack = Time.time;
                Swing();
            }

        }
    }

    private void Swing()
    {
        animator.SetTrigger("Swing");
        GameManager.instance.player.Swing();

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
