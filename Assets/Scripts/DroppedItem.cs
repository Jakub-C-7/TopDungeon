using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : Collidable
{
    [SerializeField]
    private int minQuantity;
    [SerializeField]
    private int maxQuantity;
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
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;


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
        // Make object collidable
        this.gameObject.GetComponent<BoxCollider2D>().enabled = true;

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
            int quantity = CalculateRandomRange(minQuantity, maxQuantity);
            GameManager.instance.player.inventory.coins += quantity;
            GameManager.instance.ShowText("+ " + quantity + " coins!", 25, Color.yellow, transform.position, Vector3.up * 50, 2.0f);
        }
        else if (itemType == "key")
        {

            // Set properties of key into a CollectableItem object
            GameObject keyObject = new GameObject();
            CollectableItem keyItem = keyObject.AddComponent<CollectableItem>();

            keyItem.itemName = "Dungeon Key";
            keyItem.itemType = "key";
            keyItem.itemImage = this.gameObject.GetComponent<SpriteRenderer>().sprite;

            // Move the key into the player's inventory
            keyObject.transform.parent = GameManager.instance.player.inventory.transform; //Transfer ownership of item to player's inventory
            keyObject.name = keyItem.itemName;

            // Visual text prompt for obtaining key
            GameManager.instance.ShowText(keyItem.name + " obtained!", 25, Color.cyan, transform.position, Vector3.up * 50, 2.0f);
        }

    }

    private int CalculateRandomRange(int min, int max)
    {

        int quantity = Random.Range(min, max);

        return quantity;
    }




}
