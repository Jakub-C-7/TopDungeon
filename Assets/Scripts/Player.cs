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
    private float defaultLightOuterRadius = 1.5f;
    private float lightOuterRadius = 1.5f;
    private float defaultLightInnerRadius = 0.4f;
    private bool reduceLight = false;
    private float lastBattleAction;
    private float battleModeDuration = 10f;
    private bool dead = false;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();

        ClearEquippedWeapon();
        RefreshEquippedWeapon();
        lastBattleAction = Time.time - battleModeDuration;
       
    }

    private void Update()
    {
        statusStateMachine.Update();

        movePlayer();
    }

    protected override void Death()
    {
        canMove = false;
        LightSource.pointLightOuterRadius = defaultLightInnerRadius;
       // CameraMotor cameraMotor = FindFirstObjectByType<CameraMotor>();
       // cameraMotor.GetComponent<Camera>().orthographicSize = 0.5f;
        Camera.main.orthographicSize = 0.5f;
        Camera.main.GetComponent<CameraMotor>().CentreOnPlayer();
        animator.SetTrigger("Death");
        handsAnimator.SetTrigger("Death");
        GameManager.instance.weapon.animator.SetTrigger("Death");
        dead = true;
        Invoke("ShowDeathMenu", 3f);

    }

    private void ShowDeathMenu(){
        GameManager.instance.deathMenuAnimator.SetTrigger("Show");

    }
    public void RegisterBattleAction()
    {
        lastBattleAction = Time.time;
        animator.SetBool("BattleMode", true);
        handsAnimator.SetBool("BattleMode", true);

    }

    protected override void ReceiveDamage(Damage dmg)
    {
        if (!canMove)
        {
            return;

        }

        animator.SetTrigger("Hit");
        handsAnimator.SetTrigger("Hit");
        base.ReceiveDamage(dmg);
        GameManager.instance.OnHealthChange();
        RegisterBattleAction();
    }


    private void movePlayer()
    {
        if (Time.time - lastBattleAction > battleModeDuration)
        {
            animator.SetBool("BattleMode", false);
            handsAnimator.SetBool("BattleMode", false);
            GameManager.instance.weapon.setAnimatorBool("BattleMode", false);
        }

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if (canMove)
        {
            UpdateMotor(new Vector3(x, y, 0));

        }

        float timeStep = 0.1f;

        
        if(!dead){

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

    public void RemoveAllStatusEffects(){
        statusStateMachine.RemoveAllStatusEffects();
    }

    public void Respawn()
    {
        Heal(maxHitpoints);
        canMove = true;
        dead = false;
        RemoveAllStatusEffects();
        pushDirection = Vector3.zero;
        animator.SetTrigger("Respawn");
        handsAnimator.SetTrigger("Respawn");
        GameManager.instance.weapon.animator.SetTrigger("Respawn");
    }

    public void SetReduceLight(bool reduceLight)
    {
        this.reduceLight = reduceLight;
    }

    public void RefreshEquippedWeapon()
    {
        Weapon currentWeapon = GameManager.instance.weapon;
        GameObject weaponObject = GameManager.instance.weaponObject;

        CollectableWeapon weaponToUpdateTo;

        if (equippedInventory.weapon)
        {
            weaponToUpdateTo = equippedInventory.transform.Find(equippedInventory.weapon.itemName).GetComponent<CollectableWeapon>();

            if (weaponToUpdateTo.weaponType == "Ranged")
            {

                // If ranged weapon state is disabled, enable it 
                if (weaponObject.GetComponent<RangedWeapon>().enabled == false)
                {
                    weaponObject.GetComponent<RangedWeapon>().enabled = true;

                }

                // If melee weapon state is enabled, disable it 
                if (weaponObject.GetComponent<Weapon>().enabled == true)
                {
                    weaponObject.GetComponent<Weapon>().enabled = false;

                }

                RangedWeapon rangedWeapon = weaponObject.GetComponent<RangedWeapon>();

                rangedWeapon.weaponLevel = weaponToUpdateTo.weaponLevel;
                rangedWeapon.damageAmount = weaponToUpdateTo.damageAmount;
                rangedWeapon.pushForce = weaponToUpdateTo.pushForce;

                rangedWeapon.SetWeaponImage(weaponToUpdateTo.itemImage);

                GameManager.instance.weapon = rangedWeapon; // Setting current weapon to Ranged

            }
            else if (weaponToUpdateTo.weaponType == "Melee")
            {

                // If melee weapon state is disabled, enable it 
                if (weaponObject.GetComponent<Weapon>().enabled == false)
                {
                    weaponObject.GetComponent<Weapon>().enabled = true;

                }


                // If ranged weapon state is enabled, disable it 
                if (weaponObject.GetComponent<RangedWeapon>().enabled == true)
                {
                    weaponObject.GetComponent<RangedWeapon>().enabled = false;

                }

                Weapon weapon = weaponObject.GetComponent<RangedWeapon>();

                weapon.weaponLevel = weaponToUpdateTo.weaponLevel;
                weapon.damageAmount = weaponToUpdateTo.damageAmount;
                weapon.pushForce = weaponToUpdateTo.pushForce;

                weapon.SetWeaponImage(weaponToUpdateTo.itemImage);

                GameManager.instance.weapon = weapon; // Setting current weapon to Melee

            }

            else
            {
                ClearEquippedWeapon();
            }

            weaponObject.SetActive(true);

        }

    }

    public void ClearEquippedWeapon()
    {
        GameObject weaponObject = GameManager.instance.weaponObject;

        weaponObject.gameObject.SetActive(false);

        weaponObject.gameObject.GetComponent<Weapon>().enabled = false;
        weaponObject.gameObject.GetComponent<RangedWeapon>().enabled = false;

    }

    public void Swing(string triggerName)
    {
        animator.SetTrigger(triggerName); // trigger Mover animator
        handsAnimator.SetTrigger(triggerName); // trigger Player hands animator

        RegisterBattleAction();
    }


}
