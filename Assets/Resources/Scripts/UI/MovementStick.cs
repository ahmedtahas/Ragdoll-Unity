using UnityEngine;
using UnityEngine.EventSystems;
using Unity.Netcode;
using System;

public class MovementStick : NetworkBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public RectTransform joystickBase;
    public RectTransform area;
    public RectTransform knob;
    private Vector2 inputVector;
    private Vector2 knobStartPosition;
    private Vector2 areaStartPosition;
    public event Action<Vector2> OnMove;
    int joystickSwapped = 0;



    void Start()
    {
        joystickSwapped = PlayerPrefs.GetInt("JoystickSwapped", 0);
        if (joystickSwapped == 0)
        {
            joystickBase.transform.localPosition = Constants.LEFT_STICK;
            area.transform.localPosition = Constants.LEFT_KNOB;
        }
        else
        {
            joystickBase.transform.localPosition = Constants.RIGHT_STICK;
            area.transform.localPosition = Constants.RIGHT_KNOB;
        }
    }


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
        OnMove?.Invoke(direction.normalized);
        inputVector = Vector2.ClampMagnitude(direction, area.sizeDelta.x);
        knob.position = (Vector2)area.position + inputVector;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        area.position = areaStartPosition;
        knob.position = knobStartPosition;
        inputVector = Vector2.zero;
        OnMove?.Invoke(inputVector);
    }
}