using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, 
    IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Item Item {get; set;}
    [SerializeField] private SlotItem slotItem;

    public void SetSlot(Item item)
    {
        Item = item;
        slotItem.Set(item);
    }

    public void ClearSlot()
    {
        slotItem.Clear();
        Item = null;
    }
    
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Item == null) return;
        InventorySystem.Instance.StartDrag(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        InventorySystem.Instance.UpdatePosition(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        InventorySystem.Instance.EndDrag(eventData);
    }

    public void OnDrop(PointerEventData eventData)
    {
        
    }
}
