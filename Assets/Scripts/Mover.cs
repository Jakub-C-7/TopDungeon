using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : Fighter
{
    protected BoxCollider2D boxCollider;
    protected Vector3 moveDelta;
    protected RaycastHit2D hit;
    private Vector3 originalSize;
    public float ySpeed = 0.75f;
    public float xSpeed = 1.0f;
    public Animator animator;
    protected virtual void Start()
    {
        originalSize = transform.localScale;
        boxCollider = GetComponent<BoxCollider2D>();
    }

    protected virtual void UpdateMotor(Vector3 input)
    {
        // Resetting moveDelta
        moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed, 0);

        // Swap sprite direction, right or left -------------
        if (moveDelta.x > 0)
        {
            transform.localScale = originalSize;
            if(transform.Find("HealthBar")){
                transform.Find("HealthBar").localScale = new Vector3(originalSize.x, originalSize.y, originalSize.z);
            }
        }
        else if (moveDelta.x < 0)
        {
            transform.localScale = new Vector3(originalSize.x * -1, originalSize.y, originalSize.z);
    

            if(transform.Find("HealthBar")){
                transform.Find("HealthBar").localScale = new Vector3(originalSize.x * -1, originalSize.y, originalSize.z);
            }
        }

        //Add push vector, if any
        moveDelta += pushDirection;

        // Reduce push force every frame, base off of recovery speed
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed);

        if(animator){
            float horizontalMove = input.x * xSpeed;
            float verticalMove = input.y * ySpeed;
            animator.SetFloat("Speed", Mathf.Abs(horizontalMove + verticalMove));
        }
        

        //Movement Blocking-------------
        //Make sure we can move in this direction by casting a box there first. If the box returns null, we're free to move
        //Y axis Blocking
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null)
        {
            //Make this sucker move UP!
            transform.Translate(0, moveDelta.y * Time.deltaTime, 0);
        }

        //X axis blocking
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null)
        {
            //Make this sucker move ACROSS!
            transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);
        }

    }

}
