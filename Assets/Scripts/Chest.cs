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
            // Play chest audio if it has been assigned
            if (chestAudio)
            {
                chestAudio.Play();
            }

            collected = true;
            GetComponent<SpriteRenderer>().sprite = emptyChest;
            coinAmount = Random.Range(10, 20);
            GameManager.instance.player.inventory.coins += coinAmount;
            GameManager.instance.ShowText("+ " + coinAmount + " coins!", 25, Color.yellow, transform.position, Vector3.up * 50, 2.0f);

            //Refresh the number of coins in inventory
            GameManager.instance.inventoryMenu.GetComponent<InventoryMenu>().RefreshCoins();
        }
        else if (!collected) // It is an item chest
        {
            // Try to add the item to the player's inventory
            if (OpenChest())
            {
                collected = true;

                if (chestAudio)
                {
                    chestAudio.Play();
                }

                GetComponent<SpriteRenderer>().sprite = emptyChest;

                RefreshInventory();

            }
        }

    }

    protected virtual bool OpenChest()
    {
        return GameManager.instance.TryCollectItem(item);
    }

    protected virtual void RefreshInventory()
    {
        GameManager.instance.inventoryMenu.GetComponent<InventoryMenu>().ResetAndRepopulate(item.itemType);

    }

}
