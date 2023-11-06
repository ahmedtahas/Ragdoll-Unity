using UnityEngine;
using UnityEngine.EventSystems;

public class MovementStick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public RectTransform joystickBase;
    public RectTransform knob;
    private Vector2 inputVector;
    private Vector2 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        joystickBase.position = eventData.position;
        joystickBase.gameObject.SetActive(true);
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 direction = eventData.position - (Vector2)joystickBase.position;
        inputVector = Vector2.ClampMagnitude(direction, joystickBase.sizeDelta.x / 2);
        knob.position = joystickBase.position + new Vector3(inputVector.x, inputVector.y, 0);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        joystickBase.transform.position = startPosition;
        inputVector = Vector2.zero;
    }

    public Vector2 GetInput()
    {
        return inputVector;
    }
}