using UnityEngine;
using UnityEngine.EventSystems;
using Unity.Netcode;

public class MovementStick : NetworkBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public RectTransform joystickBase;
    public RectTransform area;
    public RectTransform knob;
    private Vector2 inputVector;
    private Vector2 knobStartPosition;
    private Vector2 areaStartPosition;


    public void OnPointerDown(PointerEventData eventData)
    {
        areaStartPosition = area.position;
        knobStartPosition = knob.position;
        area.position = eventData.position;
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 direction = eventData.position - (Vector2)area.position;
        inputVector = Vector2.ClampMagnitude(direction, area.sizeDelta.x);
        knob.position = (Vector2)area.position + inputVector;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        area.position = areaStartPosition;
        knob.position = knobStartPosition;
        inputVector = Vector2.zero;
    }

    public Vector2 GetInput()
    {
        return inputVector.normalized;
    }
}