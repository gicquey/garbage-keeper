using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResourceDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    Vector3 initialPosition;
    Transform initialParent;
    Image draggedObjectImage;
    public Image imagePrefab;
    public Image duplicateObject;

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

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(eventData.pointerDrag == null)
        {
            return;
        }
        duplicateObject = Instantiate(imagePrefab, transform.parent);
        duplicateObject.raycastTarget = false;
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
            Destroy(duplicateObject.gameObject);
        }

        if (isDropped)
        {
            isDroppable = false;
            isDropped = false;
        }

    }
}
