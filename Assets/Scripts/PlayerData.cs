using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class PlayerData
{
    //References

    // Player Stats
    public int health;
    public int maxHitpoints;
    public int experience;
    public int coins;
    public int currentCharacterSelection;
    public int weaponLevel;
    //DefeatedEnemies
    public List<SpriteData> defeatedEnemies;

    // Inventory
    public List<CollectableItemStruct> consumableInventoryContents;
    public List<CollectableItemStruct> resourceInventoryContents;
    public List<CollectableItemStruct> weaponGearInventoryContents;
    public List<CollectableItemStruct> armourGearInventoryContents;
    public int consumableMaxCapacity;
    public int resourceMaxCapacity;
    public int weaponGearMaxCapacity;
    public int armourGearMaxCapacity;

    // Currently Equipped
    public List<CollectableItemStruct> equippedItems;

    public CollectableItemStruct weapon;
    public CollectableItemStruct armour;


    public PlayerData(Player player)
    {
        //Player Data
        health = player.hitPoints;
        maxHitpoints = player.maxHitpoints;
        experience = GameManager.instance.experience;
        coins = GameManager.instance.player.inventory.coins;
        currentCharacterSelection = GameManager.instance.currentCharacterSelection;
        weaponLevel = GameManager.instance.weapon.weaponLevel;
        defeatedEnemies = new List<SpriteData>();

        // Inventory data
        consumableInventoryContents = new List<CollectableItemStruct>();
        resourceInventoryContents = new List<CollectableItemStruct>();
        weaponGearInventoryContents = new List<CollectableItemStruct>();
        armourGearInventoryContents = new List<CollectableItemStruct>();

        consumableMaxCapacity = player.inventory.consumableMaxCapacity;
        resourceMaxCapacity = player.inventory.resourceMaxCapacity;
        weaponGearMaxCapacity = player.inventory.weaponGearMaxCapacity;
        armourGearMaxCapacity = player.inventory.armourGearMaxCapacity;

        LoopOverInventoryList(player.inventory.consumableInventoryContents, consumableInventoryContents);
        LoopOverInventoryList(player.inventory.resourceInventoryContents, resourceInventoryContents);
        LoopOverInventoryList(player.inventory.weaponGearInventoryContents, weaponGearInventoryContents);
        LoopOverInventoryList(player.inventory.armourGearInventoryContents, armourGearInventoryContents);

        // Currently Equipped
        equippedItems = new List<CollectableItemStruct>();

        LoopOverInventoryList(player.equippedInventory.GetComponentsInChildren<CollectableItem>().ToList(), equippedItems);

        LoopOverSpriteList(GameManager.instance.adventurerDiary.defeatedEnemies,defeatedEnemies);

    }

    private void LoopOverInventoryList(List<CollectableItem> inventoryList, List<CollectableItemStruct> targetList)
    {
        // Loop over each collectable item and retrieve its data
        foreach (CollectableItem i in inventoryList)
        {
            targetList.Add(new CollectableItemStruct
            {
                itemName = i.itemName,
                itemImage = SpriteData.FromSprite(i.itemImage),
                quantity = i.quantity,
                itemType = i.itemType
            }
        );
        }
    }

    private void LoopOverSpriteList(List<Sprite> spriteList, List<SpriteData> targetList){
        foreach(Sprite i in spriteList){
            targetList.Add(SpriteData.FromSprite(i));
        }

    }
}
