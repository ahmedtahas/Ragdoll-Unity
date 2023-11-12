using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Button joystickToggle;
    private int joystickAlignment = 0;
    private Vector3 leftStick = new Vector3(-160.0f, 0.0f, 0.0f);
    private Vector3 rightStick = new Vector3(160.0f, 0.0f, 0.0f);

    void Start()
    {
        joystickAlignment = PlayerPrefs.GetInt("JoystickSwapped", 0);
    }

    public void SwapJoystickAlignment()
    {
        joystickAlignment = joystickAlignment == 0 ? 1 : 0;
        PlayerPrefs.SetInt("JoystickSwapped", joystickAlignment);
        if (joystickAlignment == 0)
        {
            joystickToggle.transform.Find("MovementStick").transform.localPosition = rightStick;
            joystickToggle.transform.Find("SkillStick").transform.localPosition = leftStick;
        }
        else
        {
            joystickToggle.transform.Find("MovementStick").transform.localPosition = leftStick;
            joystickToggle.transform.Find("SkillStick").transform.localPosition = rightStick;
        }

    }

}
