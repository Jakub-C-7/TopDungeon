using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Collectable
{
    public Sprite emptyChest;
    public int coinAmount;
    public CollectableItem item;
    public AudioSource chestAudio;
    protected override void onCollect()
    {
        if (!collected && coinAmount > 0) // If it is a coin chest
        {
            collected = true;
            GetComponent<SpriteRenderer>().sprite = emptyChest;
            coinAmount = Random.Range(10, 20);
            GameManager.instance.player.inventory.coins += coinAmount;
            GameManager.instance.ShowText("+ " + coinAmount + " coins!", 25, Color.yellow, transform.position, Vector3.up * 50, 2.0f);
            if (chestAudio)
            {
                chestAudio.Play();
            }
        }
        else // If it is an item chest
        {
            // Try to add the item to the player's inventory
            if (GameManager.instance.TryCollectItem(item))
            {
                if (chestAudio)
                {
                    chestAudio.Play();
                }
                collected = true;
                GetComponent<SpriteRenderer>().sprite = emptyChest;

                GameManager.instance.inventoryMenu.ResetAndRepopulate(item.itemType);

            }
        }

    }

}
