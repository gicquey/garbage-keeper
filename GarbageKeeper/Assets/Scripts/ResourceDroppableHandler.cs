using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResourceDroppableHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IDropHandler
{
    private ResourceDragHandler draggableObject;
    private ResourceDragHandler currentObjectIn;

    public void OnDrop(PointerEventData eventData)
    {
        if (!draggableObject.isDropped)
        {
            if (currentObjectIn != null)
            {
                currentObjectIn.BackToInitialPosition();
            }
            if (draggableObject != null)
            {
                draggableObject.isDropped = true;
                draggableObject.isDroppable = false;
                draggableObject.transform.SetParent(transform);
                draggableObject.transform.position = transform.position;
                draggableObject.currentCanvas.sortingOrder = 1;
                currentObjectIn = draggableObject;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!eventData.dragging)
        {
            return;
        }
        if(eventData.pointerDrag != null)
        {
            draggableObject = eventData.pointerDrag.GetComponent<ResourceDragHandler>();
            draggableObject.isDroppable = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!eventData.dragging)
        {
            return;
        }

        if (draggableObject != null)
        {
            draggableObject.isDroppable = false;
        }
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
