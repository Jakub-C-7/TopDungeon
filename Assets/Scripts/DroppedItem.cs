using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : Collidable
{
    [SerializeField]
    private int quantity;
    [SerializeField]
    private string itemType;
    [SerializeField]
    private GameObject parentObject;
    [SerializeField]
    private Animator animator;

    protected override void Start()
    {
        base.Start();
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;

        if (animator)
        {
            this.gameObject.GetComponent<Animator>().enabled = false;

        }

        parentObject = this.transform.parent.gameObject;
    }

    public void DropItem()
    {
        if (animator)
        {
            this.gameObject.GetComponent<Animator>().enabled = true;

        }

        // Move to location of enemy death
        this.gameObject.transform.position = parentObject.transform.position;
        //Set parent to main scene
        this.gameObject.transform.SetParent(null);
        // Make game object visible
        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;

    }

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name == "Player")
        {
            Collect();
            Destroy(this.gameObject);

        }
    }

    private void Collect()
    {
        if (itemType == "coins")
        {
            GameManager.instance.player.inventory.coins += quantity;
            GameManager.instance.ShowText("+ " + quantity + " coins!", 25, Color.yellow, transform.position, Vector3.up * 50, 2.0f);
        }

    }




}
