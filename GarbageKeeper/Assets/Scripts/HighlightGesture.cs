using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HighlightGesture : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private Outline border;

    // Start is called before the first frame update
    void Start()
    {
        border = GetComponent<Outline>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        border.effectColor = new Color(border.effectColor.r, border.effectColor.g, border.effectColor.b, 1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        border.effectColor = new Color(border.effectColor.r, border.effectColor.g, border.effectColor.b, 0);
    }
}
