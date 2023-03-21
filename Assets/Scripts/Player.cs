using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Player : Mover
{
    public SpriteRenderer spriteRenderer;
    public Light2D LightSource;
    public Inventory inventory;
    public EquippedInventory equippedInventory;
    private float horizontalMove;
    private float verticalMove;

    public bool canMove = true;
    private float defaultLightOuterRadius = 1.5f;
    private float lightOuterRadius = 1.5f;
    private bool reduceLight = false;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();

        ClearEquippedWeapon();
        RefreshEquippedWeapon();

    }

    protected override void Death()
    {
        canMove = false;
        GameManager.instance.deathMenuAnimator.SetTrigger("Show");
    }

    protected override void ReceiveDamage(Damage dmg)
    {
        if (!canMove)
        {
            return;

        }
        base.ReceiveDamage(dmg);
        GameManager.instance.OnHealthChange();
    }

    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if (canMove)
        {
            UpdateMotor(new Vector3(x, y, 0));

        }

        float timeStep = 0.1f;
        if (reduceLight)
        {
            if (lightOuterRadius > 0)
            {
                lightOuterRadius -= timeStep;
            }
        }
        else
        {
            if (lightOuterRadius < defaultLightOuterRadius)
            {
                lightOuterRadius += timeStep;
            }
            else if (lightOuterRadius > defaultLightOuterRadius)
            {
                lightOuterRadius = defaultLightOuterRadius;
            }
        }
        LightSource.pointLightOuterRadius = lightOuterRadius;
    }

    public void SwapSprite(int skinId)
    {
        GameManager.instance.currentCharacterSelection = skinId; //Set the current skin ID
        spriteRenderer.sprite = GameManager.instance.playerSprites[skinId];
        Animator animator = GameManager.instance.player.transform.gameObject.GetComponent<Animator>(); //Change the animation
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

    public void Respawn()
    {
        Heal(maxHitpoints);
        canMove = true;
        lastImmune = Time.time;
        pushDirection = Vector3.zero;
    }

    public void SetReduceLight(bool reduceLight)
    {
        this.reduceLight = reduceLight;
    }

    public void RefreshEquippedWeapon()
    {
        Weapon currentWeapon = GameManager.instance.weapon;

        if (equippedInventory.weapon != null)
        {
            Debug.Log("RefreshEquippedWeapon");

            CollectableWeapon weaponToUpdateTo = equippedInventory.transform.Find(equippedInventory.weapon.itemName).GetComponent<CollectableWeapon>();
            currentWeapon.weaponLevel = weaponToUpdateTo.weaponLevel;
            currentWeapon.damageAmount = weaponToUpdateTo.damageAmount;
            currentWeapon.pushForce = weaponToUpdateTo.pushForce;

            // SpriteData spriteData = SpriteData.FromSprite(weaponToUpdateTo.itemImage);

            // // Centre the weapon sprite into the middle of the parent
            // spriteData.pivotX = 0.5f;
            // spriteData.pivotY = 0.5f;

            // Sprite weaponSprite = SpriteData.ToSprite(spriteData);

            currentWeapon.SetWeaponImage(weaponToUpdateTo.itemImage);

        }

    }

    public void ClearEquippedWeapon()
    {
        Weapon currentWeapon = this.transform.Find("Weapon").GetComponent<Weapon>();

        if (equippedInventory.weapon != null)
        {

            currentWeapon.gameObject.GetComponent<SpriteRenderer>().sprite = null;

        }
    }


}
