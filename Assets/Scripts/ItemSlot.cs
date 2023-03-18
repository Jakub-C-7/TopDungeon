using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ItemSlot : MonoBehaviour, IDropHandler
{

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            //Transfer ownership of item to the item slot
            eventData.pointerDrag.GetComponent<RectTransform>().transform.SetParent(GetComponent<RectTransform>().transform); // Transfer ownership of object

            // Set the anchor of the item
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;

            // Centralise the item image in the item slot
            eventData.pointerDrag.GetComponent<RectTransform>().transform.localPosition = Vector3.zero;

            //TODO: Check if the item is being equipped.

            //If being equipped, remove from inventory,

            // Refresh the currently equipped items
            GameManager.instance.player.equippedInventory.RefreshEquippedItems();
        }
    }
}
