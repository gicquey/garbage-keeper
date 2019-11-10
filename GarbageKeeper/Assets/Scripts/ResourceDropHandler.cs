using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResourceDropHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    private ResourceDragHandler draggedBaseObject;
    private ResourceItem currentObjectIn;
    public int slotNumber;

    public void OnDrop(PointerEventData eventData)
    {

        if (draggedBaseObject != null && InventoryManager.Instance.getQuantityForGivenResource(draggedBaseObject.duplicateObject.GetComponent<ResourceItem>().resourceType) > 0)
        {
            if (currentObjectIn != null)
            {
                Destroy(currentObjectIn.gameObject);
            }
            draggedBaseObject.isDropped = true;
            draggedBaseObject.duplicateObject.transform.SetParent(transform);
            draggedBaseObject.duplicateObject.transform.position = transform.position;
            draggedBaseObject.duplicateObject.GetComponent<ResourceItem>().currentSlot = slotNumber;
            draggedBaseObject.duplicateObject.raycastTarget = true;
            currentObjectIn = draggedBaseObject.duplicateObject.GetComponent<ResourceItem>();
            CraftManager.Instance.addResource(slotNumber, draggedBaseObject.duplicateObject.gameObject);
        }
        else
        {
            Destroy(draggedBaseObject.duplicateObject);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!eventData.dragging)
        {
            return;
        }
        if (eventData.pointerDrag != null)
        {
            draggedBaseObject = eventData.pointerDrag.GetComponent<ResourceDragHandler>();
            draggedBaseObject.isDroppable = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!eventData.dragging)
        {
            return;
        }

        draggedBaseObject.isDroppable = false;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
