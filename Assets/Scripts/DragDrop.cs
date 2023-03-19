using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] public Canvas canvas;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Transform originalParent;
    private GameObject bagPanel;

    private void Awake()
    {
        bagPanel = GameObject.Find("BagPanel");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        // set the original position and parent of the item
        // originalPosition = rectTransform.anchoredPosition; // Not needed anymore
        originalParent = eventData.pointerDrag.transform.parent;

        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;

        // Set as last child of the bag panel until dropped onto a slot
        eventData.pointerDrag.transform.SetParent(bagPanel.transform);
        eventData.pointerDrag.transform.SetAsLastSibling();

    }

    public void OnDrag(PointerEventData eventData)
    {
        // Make the item follow the cursor
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // References
        GameObject itemBeingDragged = eventData.pointerDrag.gameObject;
        GameObject itemBeingDraggedOnto = eventData.pointerCurrentRaycast.gameObject;
        string raycastParentName = itemBeingDraggedOnto.transform.parent.name;
        bool emptyBagSlot = eventData.pointerCurrentRaycast.gameObject.name.Contains("ItemHolster");

        // If dragged onto the same slot, Do nothing
        if (originalParent.name == eventData.pointerCurrentRaycast.gameObject.name)
        {
            returnDraggedItemToOrigin(eventData);
        }
        //  Dropped onto another item and it is the same type of item
        else if (itemBeingDraggedOnto.GetComponent<CollectableItem>() && itemBeingDraggedOnto.GetComponent<CollectableItem>().itemType == itemBeingDragged.GetComponent<CollectableItem>().itemType)
        {

            //If the item being dropped on is equipped, swap the two items around
            if (raycastParentName == "WeaponHolster" || raycastParentName == "ArmourHolster" || raycastParentName == "ConsumableHolsterOne" || raycastParentName == "ConsumableHolsterTwo")
            {
                Debug.Log("An item is already equipped but we are trying to equip a new one!");

                // Un-equip currently equipped item
                GameManager.instance.player.equippedInventory.UnEquipItem(itemBeingDraggedOnto.transform.parent, itemBeingDraggedOnto.GetComponent<CollectableItem>());

                // Equip new item
                GameManager.instance.player.equippedInventory.EquipItem(itemBeingDragged.GetComponent<CollectableItem>());

            }

            // Switch the parents of the two items
            eventData.pointerDrag.transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent);
            eventData.pointerCurrentRaycast.gameObject.transform.SetParent(originalParent);

            // Reset both item positions to the middle - Switching the item's positions
            eventData.pointerDrag.GetComponent<RectTransform>().transform.localPosition = Vector3.zero;
            eventData.pointerCurrentRaycast.gameObject.transform.localPosition = Vector3.zero;

        }
        // Item has been dragged onto an empty item slot and the currently selected tab is the same as the item's type
        else if ((emptyBagSlot && eventData.pointerCurrentRaycast.gameObject.GetComponent<ItemSlot>()) && GameManager.instance.inventoryMenu.GetComponent<InventoryMenu>().currentlySelectedTab == eventData.pointerDrag.GetComponent<CollectableItem>().itemType)
        {

            // If item was equipped, unequip it
            if ((originalParent.name == "WeaponHolster" || originalParent.name == "ArmourHolster" || originalParent.name == "ConsumableHolsterOne" || originalParent.name == "ConsumableHolsterTwo"))
            {
                Debug.Log("We are unequipping an equipped object onto an empty bag slot: " + emptyBagSlot);
                // Un-equip currently equipped item
                GameManager.instance.player.equippedInventory.UnEquipItem(originalParent, itemBeingDragged.GetComponent<CollectableItem>());
            }

        }
        // Item has been dropped onto an item slot
        else if (eventData.pointerCurrentRaycast.gameObject.GetComponent<ItemSlot>())
        {
            Debug.Log("Item has been dropped onto an item slot" + eventData.pointerCurrentRaycast.gameObject.name);

            // Checks to ensure that the correct type of item has been dropped onto the correct slot
            switch (eventData.pointerCurrentRaycast.gameObject.name)
            {

                case "WeaponHolster" when eventData.pointerDrag.GetComponent<CollectableItem>().itemType == "Weapon":
                    Debug.Log("equipping weapon onto weapon holster");

                    GameManager.instance.player.equippedInventory.EquipItem(eventData.pointerDrag.GetComponent<CollectableItem>());
                    break;

                case "ArmourHolster" when eventData.pointerDrag.GetComponent<CollectableItem>().itemType == "Armour":
                    Debug.Log("equipping armour onto armour holster");

                    GameManager.instance.player.equippedInventory.EquipItem(eventData.pointerDrag.GetComponent<CollectableItem>());
                    break;

                case "ConsumableHolsterOne" when eventData.pointerDrag.GetComponent<CollectableItem>().itemType == "Consumable":

                    GameManager.instance.player.equippedInventory.EquipItem(eventData.pointerDrag.GetComponent<CollectableItem>());
                    break;

                case "ConsumableHolsterTwo" when eventData.pointerDrag.GetComponent<CollectableItem>().itemType == "Consumable":

                    GameManager.instance.player.equippedInventory.EquipItem(eventData.pointerDrag.GetComponent<CollectableItem>());
                    break;

                default:

                    returnDraggedItemToOrigin(eventData);
                    break;

            }


        }

        else if (!eventData.pointerCurrentRaycast.gameObject.GetComponent<ItemSlot>())
        {
            returnDraggedItemToOrigin(eventData);
        }

    }

    public void returnDraggedItemToOrigin(PointerEventData eventData)
    {
        eventData.pointerDrag.transform.SetParent(originalParent);
        eventData.pointerDrag.GetComponent<RectTransform>().transform.localPosition = Vector3.zero;
    }


}
