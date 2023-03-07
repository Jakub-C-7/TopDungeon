using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mover
{
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    private float horizontalMove;
    private float verticalMove;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    protected override void ReceiveDamage(Damage dmg)
    {
        base.ReceiveDamage(dmg);
        GameManager.instance.OnHealthChange();
    }

    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        UpdateMotor(new Vector3(x, y, 0));

        horizontalMove = x * xSpeed;
        verticalMove = y * ySpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove + verticalMove));
    }

    public void SwapSprite(int skinId)
    {
        spriteRenderer.sprite = GameManager.instance.playerSprites[skinId];
        //Change the animation
        Animator animator = GameManager.instance.player.transform.gameObject.GetComponent<Animator>();
        animator.runtimeAnimatorController = Resources.Load("Animations/Player_" + skinId) as RuntimeAnimatorController;
    }

    public void OnLevelUp()
    {
        maxHitpoints++;
        hitPoints = maxHitpoints;
    }

    public void SetLevel(int level)
    {
        for (int i = 0; i < level; i++)
        {
            OnLevelUp();
        }
    }

    public void Heal(int healingAmount)
    {

        if (hitPoints + healingAmount > maxHitpoints)
        {
            hitPoints = maxHitpoints;
        }
        else
        {
            hitPoints += healingAmount;
        }

        GameManager.instance.ShowText("+ " + healingAmount.ToString() + "hp", 25, Color.green, transform.position, Vector3.up * 30, 1.0f);
        GameManager.instance.OnHealthChange();

    }


}
