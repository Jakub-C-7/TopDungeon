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

        // set the original position / parent of the item
        // originalPosition = rectTransform.anchoredPosition;
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

        // If the item has been dropped onto another item
        if (eventData.pointerCurrentRaycast.gameObject.GetComponent<CollectableItem>())
        {
            // Switch the parents of the two items
            eventData.pointerDrag.transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent);
            eventData.pointerCurrentRaycast.gameObject.transform.SetParent(originalParent);

            // Reset both item positions to the middle - Switching the item's positions
            eventData.pointerDrag.GetComponent<RectTransform>().transform.localPosition = Vector3.zero;
            eventData.pointerCurrentRaycast.gameObject.transform.localPosition = Vector3.zero;

        }
        // If the item has not been dropped onto an item slot or another item
        else if (!eventData.pointerCurrentRaycast.gameObject.GetComponent<ItemSlot>())
        {
            eventData.pointerDrag.transform.SetParent(originalParent);
            eventData.pointerDrag.GetComponent<RectTransform>().transform.localPosition = Vector3.zero;

        }

    }


}
