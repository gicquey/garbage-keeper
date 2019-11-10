using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableAmmo : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Camera mainCam;
    public Settings.AmmoType ammoType;
    public int droppedQuantity;
    private Vector3 initialPos;
    private RectTransform rectTransform;
    private VerticalLayoutGroup vertical;
    private Image currentImage;

    public void OnBeginDrag(PointerEventData eventData)
    {
        currentImage.raycastTarget = false;
        AmmoDragManager.Instance.isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (AmmoDragManager.Instance.canDrop)
        {
            AmmoDragManager.Instance.dropAmmo(ammoType, droppedQuantity);
        }

        currentImage.raycastTarget = true;
        rectTransform.localPosition = initialPos;
        LayoutRebuilder.ForceRebuildLayoutImmediate(vertical.GetComponent<RectTransform>());
    }

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        vertical = GetComponentInParent<VerticalLayoutGroup>();
        currentImage = GetComponent<Image>();
        initialPos = rectTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
