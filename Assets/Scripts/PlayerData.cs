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
    public List<CollectableWeaponStruct> weaponGearInventoryContents;
    public List<CollectableArmourStruct> armourGearInventoryContents;
    public int consumableMaxCapacity;
    public int resourceMaxCapacity;
    public int weaponGearMaxCapacity;
    public int armourGearMaxCapacity;

    // Currently Equipped
    public CollectableWeaponStruct equippedWeapon;
    public CollectableArmourStruct equippedArmour;


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
        weaponGearInventoryContents = new List<CollectableWeaponStruct>();
        armourGearInventoryContents = new List<CollectableArmourStruct>();

        consumableMaxCapacity = player.inventory.consumableMaxCapacity;
        resourceMaxCapacity = player.inventory.resourceMaxCapacity;
        weaponGearMaxCapacity = player.inventory.weaponGearMaxCapacity;
        armourGearMaxCapacity = player.inventory.armourGearMaxCapacity;

        LoopOverInventoryList(player.inventory.consumableInventoryContents, consumableInventoryContents);
        LoopOverInventoryList(player.inventory.resourceInventoryContents, resourceInventoryContents);
        LoopOverWeaponsInInventoryList(player.inventory.weaponGearInventoryContents, weaponGearInventoryContents);
        LoopOverArmourInInventoryList(player.inventory.armourGearInventoryContents, armourGearInventoryContents);

        // Currently Equipped Inventory
        GetCurrentlyEquippedWeaponData(player);
        GetCurrentlyEquippedArmourData(player);

        // TODO: EQUIPPED CONSUMABLE SAVING

        LoopOverSpriteList(GameManager.instance.adventurerDiary.defeatedEnemies, defeatedEnemies);

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

    public void LoopOverWeaponsInInventoryList(List<CollectableWeapon> inventoryList, List<CollectableWeaponStruct> targetList)
    {
        //Loading weapons in inventory details
        foreach (CollectableWeapon i in inventoryList)
        {
            targetList.Add(new CollectableWeaponStruct
            {
                itemName = i.itemName,
                itemImage = SpriteData.FromSprite(i.itemImage),
                quantity = i.quantity,
                itemType = i.itemType,

                pushForce = i.pushForce,
                weaponLevel = i.weaponLevel,
                damageAmount = i.damageAmount
            });

        }
    }

    public void LoopOverArmourInInventoryList(List<CollectableArmour> inventoryList, List<CollectableArmourStruct> targetList)
    {
        //Loading weapons in inventory details
        foreach (CollectableArmour i in inventoryList)
        {
            targetList.Add(new CollectableArmourStruct
            {
                itemName = i.itemName,
                itemImage = SpriteData.FromSprite(i.itemImage),
                quantity = i.quantity,
                itemType = i.itemType,

                protectionAmount = i.protectionAmount,
                armourLevel = i.armourLevel,
                specialEffect = i.specialEffect
            });

        }
    }

    private void GetCurrentlyEquippedWeaponData(Player player)
    {

        if (player.equippedInventory.weapon)
        {

            CollectableWeapon currentWeapon = player.equippedInventory.weapon.GetComponent<CollectableWeapon>();
            equippedWeapon.itemName = currentWeapon.itemName;
            equippedWeapon.quantity = currentWeapon.quantity;
            equippedWeapon.itemType = currentWeapon.itemType;
            equippedWeapon.itemImage = SpriteData.FromSprite(currentWeapon.itemImage);
            equippedWeapon.weaponType = currentWeapon.weaponType;

            equippedWeapon.weaponLevel = currentWeapon.weaponLevel;
            equippedWeapon.damageAmount = currentWeapon.damageAmount;
            equippedWeapon.pushForce = currentWeapon.pushForce;
        }

    }

    private void GetCurrentlyEquippedArmourData(Player player)
    {

        if (player.equippedInventory.armour)
        {
            CollectableArmour currentArmour = player.equippedInventory.armour.GetComponent<CollectableArmour>();
            equippedArmour.itemName = currentArmour.itemName;
            equippedArmour.itemImage = SpriteData.FromSprite(currentArmour.itemImage);
            equippedArmour.quantity = currentArmour.quantity;
            equippedArmour.itemType = currentArmour.itemType;

            equippedArmour.armourLevel = currentArmour.armourLevel;
            equippedArmour.protectionAmount = currentArmour.protectionAmount;
            equippedArmour.specialEffect = currentArmour.specialEffect;
        }
    }

    private void LoopOverSpriteList(List<Sprite> spriteList, List<SpriteData> targetList)
    {
        foreach (Sprite i in spriteList)
        {
            targetList.Add(SpriteData.FromSprite(i));
        }

    }
}
