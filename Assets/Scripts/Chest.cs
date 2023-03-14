using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Collectable
{
    public Sprite emptyChest;
    public int coinAmount;
    public CollectableItem item;
    protected override void onCollect()
    {
        if (!collected && coinAmount > 0) // If it is a coin chest
        {
            collected = true;
            GetComponent<SpriteRenderer>().sprite = emptyChest;
            coinAmount = Random.Range(10, 20);
            GameManager.instance.coins += coinAmount;
            GameManager.instance.ShowText("+ " + coinAmount + " coins!", 25, Color.yellow, transform.position, Vector3.up * 50, 2.0f);
        }
        else // If it is an item chest
        {
            collected = true;
            GetComponent<SpriteRenderer>().sprite = emptyChest;
            GameManager.instance.CollectItem(item); //Add the item to the player's inventory
            GameManager.instance.ShowText(item.itemName + " acquired!", 20, Color.cyan, transform.position, Vector3.up * 50, 2.0f);

        }

    }

}
