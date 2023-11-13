using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.EventSystems;
using System;

public class SkillStick : NetworkBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public RectTransform joystickBase;
    public RectTransform area;
    public RectTransform knob;
    private Vector2 inputVector;
    private Vector2 knobStartPosition;
    private Vector2 areaStartPosition;
    private float chargeStartTime;

    public enum BehaviorType { Click, ChargeUp, AimAndRelease }
    public BehaviorType currentBehavior;

    // Define an event to send out the signal
    public event Action<Vector2, bool> OnAim;
    public event Action<bool, float> OnChargeUp;
    public event Action OnClick;
    int joystickSwapped = 0;

    void Start()
    {
        joystickSwapped = PlayerPrefs.GetInt("JoystickSwapped", 0);
        if (joystickSwapped == 1)
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

    public void SetBehavior(BehaviorType behavior)
    {
        currentBehavior = behavior;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        areaStartPosition = area.position;
        knobStartPosition = knob.position;
        area.position = eventData.position;
        switch (currentBehavior)
        {
            case BehaviorType.Click:
                OnClick?.Invoke();
                break;
            case BehaviorType.ChargeUp:
                chargeStartTime = Time.time;
                OnChargeUp?.Invoke(false, 0.0f);
                break;
            case BehaviorType.AimAndRelease:
                break;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentBehavior != BehaviorType.AimAndRelease)
        {
            return;
        }
        Vector2 direction = eventData.position - (Vector2)area.position;
        inputVector = Vector2.ClampMagnitude(direction, area.sizeDelta.x);
        knob.position = (Vector2)area.position + inputVector;
        OnAim?.Invoke(inputVector.normalized, false);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        switch (currentBehavior)
        {
            case BehaviorType.ChargeUp:
                float chargeTime = Time.time - chargeStartTime;
                OnChargeUp?.Invoke(true, chargeTime);
                chargeStartTime = 0.0f;
                break;
            case BehaviorType.AimAndRelease:
                OnAim?.Invoke(inputVector.normalized, true);
                break;
        }
        area.position = areaStartPosition;
        knob.position = knobStartPosition;
    }
}