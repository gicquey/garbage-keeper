using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResourceDropHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    public ResourceDragHandler draggedBaseObject;
    public ResourceItem currentObjectIn;
    public int slotNumber;


    bool isOver = false;

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
            CraftManager.Instance.addResource(slotNumber, draggedBaseObject.duplicateObject.GetComponentInParent<ResourceDropHandler>());
        }
        else
        {
            Destroy(draggedBaseObject.duplicateObject);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isOver = true;

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
        isOver = false;
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
       if(Input.GetMouseButtonDown(1) && isOver)
        {
            RemoveResource();
        }
    }

    public void RemoveResource()
    {
        draggedBaseObject.isDroppable = false;
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
