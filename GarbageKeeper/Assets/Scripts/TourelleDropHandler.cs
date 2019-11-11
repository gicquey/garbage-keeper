using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TourelleDropHandler : MonoBehaviour
{
    private bool isOver;
    public MeshRenderer garbageRenderer;
    private Material material;
    private Color baseColor;

    private void OnMouseOver()
    {
        if (AmmoDragManager.Instance.isDragging)
        {
            material.color = Color.green;
            AmmoDragManager.Instance.canDrop = true;
            AmmoDragManager.Instance.dropZone = GetComponent<Tourelle>();
        }
    }
    

    private void OnMouseExit()
    {
        material.color = baseColor;
        if (AmmoDragManager.Instance.isDragging)
        {
            AmmoDragManager.Instance.canDrop = false;
            AmmoDragManager.Instance.dropZone = null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        material = garbageRenderer.material;
        baseColor = material.color;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
