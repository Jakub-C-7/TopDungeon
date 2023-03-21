using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChest : Chest
{
    public CollectableWeapon collectableWeapon;

    protected override bool OpenChest()
    {
        return GameManager.instance.TryCollectWeapon(collectableWeapon);
    }

    protected override void RefreshInventory()
    {
        GameManager.instance.inventoryMenu.GetComponent<InventoryMenu>().ResetAndRepopulate(collectableWeapon.itemType);
    }

}
