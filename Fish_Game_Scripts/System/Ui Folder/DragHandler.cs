using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Action<PointerEventData> OnBeginDragEvent;

    public Action<PointerEventData> OnDragEvent;

    public Action<PointerEventData> OnEndDragEvent;

    [SerializeField] private Canvas UICanvas;

    public void OnBeginDrag(PointerEventData eventData)
    {
        OnBeginDragEvent?.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnDragEvent?.Invoke(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnEndDragEvent?.Invoke(eventData);
    }

    public Vector2 GetPositionOnCanvas(Vector2 pointerPos)
    {
        if (UICanvas == null)
        {
            UICanvas = GetBelongedCanvas(transform);
        }
        RectTransformUtility.ScreenPointToLocalPointInRectangle(UICanvas.transform as RectTransform, pointerPos, UICanvas.worldCamera, out Vector2 localPointerPos);
        return localPointerPos;
    }

    private Canvas GetBelongedCanvas(Transform t)
    {
        if (t == null)
        {
            return null;
        }

        var canvas = t.GetComponent<Canvas>();
        if (canvas != null)
        {
            return canvas;
        }

        return GetBelongedCanvas(t.parent);
    }

}
