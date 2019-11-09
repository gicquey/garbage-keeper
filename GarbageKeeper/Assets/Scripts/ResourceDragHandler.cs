using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResourceDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{

    Vector3 initialPosition;
    Transform initialParent;
    Image draggedObjectImage;
    bool isOver = false;
    public Canvas currentCanvas;
    public bool isDroppable { get; set; }
    public bool isDropped { get; set; }
    
    // Start is called before the first frame update
    void Start()
    {
        currentCanvas = GetComponent<Canvas>();
        initialPosition = transform.localPosition;
        initialParent = transform.parent;
        draggedObjectImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && isOver)
        {
            BackToInitialPosition();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        draggedObjectImage.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDropped)
        {
            transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDroppable)
        {
            currentCanvas.sortingOrder = 1;
            transform.localPosition = initialPosition;
        }

        draggedObjectImage.raycastTarget = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isOver = false;
    }

    public void BackToInitialPosition()
    {
        isDroppable = false;
        isDropped = false;
        currentCanvas.sortingOrder = 1;
        transform.SetParent(initialParent);
        transform.localPosition = initialPosition;
    }
    
}
