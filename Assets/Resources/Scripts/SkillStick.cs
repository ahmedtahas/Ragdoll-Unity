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
        // if (!IsOwner)
        // {
        //     gameObject.SetActive(false);
        //     return;
        // }
        startPosition = transform.position;
    }

    public void SetBehavior(BehaviorType behavior)
    {
        currentBehavior = behavior;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // if (!IsOwner)
        // {
        //     return;
        // }

        switch (currentBehavior)
        {
            case BehaviorType.Click:
                // Handle click behavior
                Debug.Log("Clicked");
                OnClick?.Invoke(true);
                break;
            case BehaviorType.ChargeUp:
                // Start charging up
                isCharging = true;
                chargeStartTime = Time.time;
                Debug.Log("Charging up");
                OnChargeUp?.Invoke(false, 0.0f);
                break;
            case BehaviorType.AimAndRelease:
                // Start aiming
                Debug.Log("Aiming");
                break;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // if (!IsOwner || currentBehavior != BehaviorType.AimAndRelease)
        // {
        //     return;
        // }

        // Update aim
        inputVector = eventData.position - startPosition;
        Debug.Log("Aiming");

        // Send out the signal
        OnAim?.Invoke(inputVector, false);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // if (!IsOwner)
        // {
        //     return;
        // }

        switch (currentBehavior)
        {
            case BehaviorType.ChargeUp:
                // Release charge
                isCharging = false;
                Debug.Log("Released charge");
                float chargeTime = Time.time - chargeStartTime;
                OnChargeUp?.Invoke(true, chargeTime);
                chargeStartTime = 0.0f;
                break;
            case BehaviorType.AimAndRelease:
                // Release aim
                Debug.Log("Released aim");
                // Send out the signal
                OnAim?.Invoke(inputVector, true);
                break;
        }
    }
}