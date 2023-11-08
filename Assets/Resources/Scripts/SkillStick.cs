using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.EventSystems;
using System;

public class SkillStick : NetworkBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public RectTransform joystickBase;
    public RectTransform knob;
    private Vector2 inputVector;
    private Vector2 startPosition;
    private bool isCharging = false;
    private float chargeStartTime;

    public enum BehaviorType { Click, ChargeUp, AimAndRelease }
    public BehaviorType currentBehavior;

    // Define an event to send out the signal
    public event Action<Vector2, bool> OnAim;
    public event Action<bool, float> OnChargeUp;
    public event Action<bool> OnClick;

    void Start()
    {
        startPosition = transform.position;
    }

    public void SetBehavior(BehaviorType behavior)
    {
        currentBehavior = behavior;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        switch (currentBehavior)
        {
            case BehaviorType.Click:
                OnClick?.Invoke(true);
                break;
            case BehaviorType.ChargeUp:
                isCharging = true;
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
        inputVector = eventData.position - startPosition;
        OnAim?.Invoke(inputVector.normalized, false);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        switch (currentBehavior)
        {
            case BehaviorType.ChargeUp:
                isCharging = false;
                float chargeTime = Time.time - chargeStartTime;
                OnChargeUp?.Invoke(true, chargeTime);
                chargeStartTime = 0.0f;
                break;
            case BehaviorType.AimAndRelease:
                OnAim?.Invoke(inputVector.normalized, true);
                break;
        }
    }
}