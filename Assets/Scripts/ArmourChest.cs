using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourChest : Chest
{
    public CollectableArmour collectableArmour;

    protected override bool OpenChest()
    {
        return GameManager.instance.TryCollectArmour(collectableArmour);
    }

    protected override void RefreshInventory()
    {
        GameManager.instance.inventoryMenu.GetComponent<InventoryMenu>().ResetAndRepopulate(collectableArmour.itemType);
    }
}
