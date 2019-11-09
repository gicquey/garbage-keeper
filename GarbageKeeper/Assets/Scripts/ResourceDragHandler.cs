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
    public Image duplicateObject;

    bool isOver = false;
    public Canvas currentCanvas;
    public bool isDroppable { get; set; }
    public bool isDropped { get; set; }
    public int currentSlot;
    
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
            CraftManager.Instance.removeResource(currentSlot);
            currentSlot = -1;
            BackToInitialPosition();
        }

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(eventData.pointerDrag == null)
        {
            return;
        }
        duplicateObject = Instantiate(, transform.parent);
        duplicateObject.raycastTarget = false;
        eventData.pointerDrag = duplicateObject.gameObject;
    }

    public void OnDrag(PointerEventData eventData)
    {
        duplicateObject.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDroppable)
        {
            duplicateObject.raycastTarget = true;
            currentCanvas.sortingOrder = 1;
            transform.localPosition = initialPosition;
        }

        Destroy(duplicateObject.gameObject);
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
