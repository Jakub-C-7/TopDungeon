using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : Fighter
{
    public BoxCollider2D boxCollider;
    protected Vector3 moveDelta;
    protected RaycastHit2D hit;
    private Vector3 originalSize;
    public float ySpeed = 0.75f;
    public float xSpeed = 1.0f;
    public Animator animator;
    public Animator handsAnimator;
    public bool canMove = true;
    public bool staggered = false;

    protected StatusStateMachine statusStateMachine;

    protected virtual void Start()
    {
        statusStateMachine = new StatusStateMachine(this);
        originalSize = transform.localScale;
        boxCollider = GetComponent<BoxCollider2D>();
    }

    protected virtual void RecieveStatusEffect(StatusEffect statusEffect)
    {
        statusStateMachine.AddState(statusEffect.statusState, Time.time + statusEffect.duration);
    }

    public virtual void UpdateMotor(Vector3 input)
    {
        // Resetting moveDelta
        moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed, 0);

        // Swap sprite direction, right or left -------------
        if (moveDelta.x > 0)
        {
            // Keep sprite oriented in the same way
            transform.localScale = originalSize;

            if (transform.Find("HealthBar"))
            {
                transform.Find("HealthBar").localScale = new Vector3(originalSize.x, originalSize.y, originalSize.z);
            }
        }

        else if (moveDelta.x < 0)
        {
            // Flip sprite
            transform.localScale = new Vector3(originalSize.x * -1, originalSize.y, originalSize.z);

            if (transform.Find("HealthBar"))
            {
                transform.Find("HealthBar").localScale = new Vector3(originalSize.x * -1, originalSize.y, originalSize.z);
            }
        }

        float horizontalMove = input.x * xSpeed;
        float verticalMove = input.y * ySpeed;

        // Setting animator values for Player
        if (animator && this.name == "Player")
        {


            // If the mover is moving up.
            if (moveDelta.y > 0)
            {
                SetMoverAnimators("MovingUp", true);

            }
            else
            {
                SetMoverAnimators("MovingUp", false);

            }

            // If the mover is moving down
            if (moveDelta.y < 0)
            {
                SetMoverAnimators("MovingDown", true);

            }
            else
            {
                SetMoverAnimators("MovingDown", false);

            }


            if (handsAnimator)
            {
                handsAnimator.SetFloat("Speed", Mathf.Abs(horizontalMove + verticalMove));
            }

            if (GameManager.instance.weapon.animator)
            {
                GameManager.instance.weapon.animator.SetFloat("Speed", Mathf.Abs(horizontalMove + verticalMove));
            }
            //Add push vector, if any


        }
        //Add push vector, if any
        moveDelta += pushDirection;

        // Reduce push force every frame, base off of recovery speed
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed);

        if (animator)
        {

            animator.SetFloat("Speed", Mathf.Abs(horizontalMove + verticalMove));

        }

        //Movement Blocking-------------
        //Make sure we can move in this direction by casting a box there first. If the box returns null, we're free to move
        //Y axis Blocking
        hit = Physics2D.BoxCast(transform.position + (Vector3)boxCollider.offset, boxCollider.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null)
        {
            //Make this sucker move UP!
            transform.Translate(0, moveDelta.y * Time.deltaTime, 0);
        }

        //X axis blocking
        hit = Physics2D.BoxCast(transform.position + (Vector3)boxCollider.offset, boxCollider.size, 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null)
        {
            //Make this sucker move ACROSS!
            transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);
        }

    }

    private void SetMoverAnimators(string paramName, bool toggled)
    {
        if (animator)
        {
            animator.SetBool(paramName, toggled);

        }

        if (handsAnimator)
        {
            handsAnimator.SetBool(paramName, toggled);

        }

        if (GameManager.instance.player.weaponAnimator)
        {
            // GameManager.instance.weapon.setAnimatorBool(paramName, toggled);
            GameManager.instance.player.weaponAnimator.SetBool(paramName, toggled);

        }

    }



    protected virtual void Stagger()
    {
        canMove = false;

        // cannot attack
        staggered = true;

    }

    protected virtual void RecoverFromStagger()
    {
        canMove = true;

        // can attack attack
        staggered = false;

    }

    void Update()
    {
        statusStateMachine.Update();
    }

}

