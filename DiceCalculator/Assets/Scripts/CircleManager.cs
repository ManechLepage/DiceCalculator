using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CircleManager : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public bool isDragable = true;

    private RectTransform rectTransform;
    private Vector2 pointerOffset;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out pointerOffset);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragable)
        {
            Vector2 pointerPosition = eventData.position - pointerOffset;
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, pointerPosition.y);
        }
    }
}
