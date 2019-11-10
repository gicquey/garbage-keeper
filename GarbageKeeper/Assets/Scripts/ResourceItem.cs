using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResourceItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Settings.Elements resourceType;
    public int currentSlot = -1;

    private bool isOver = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isOver = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && isOver && currentSlot > 0)
        {
            RemoveResource();
        }
    }

    public void RemoveResource()
    {
        CraftManager.Instance.removeResource(currentSlot);
        Destroy(this.gameObject);
    }
}
