using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickVirtual : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public RectTransform background;
    public RectTransform handle;

    [HideInInspector] public Vector2 inputDirection;

    private Vector2 center;

    void Start()
    {
        center = background.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background, eventData.position, eventData.pressEventCamera, out pos);

        pos = Vector2.ClampMagnitude(pos, background.sizeDelta.x / 2f);
        handle.anchoredPosition = pos;

        inputDirection = pos / (background.sizeDelta.x / 2f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputDirection = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }

    public float Horizontal() => inputDirection.x;
    public float Vertical() => inputDirection.y;
}
