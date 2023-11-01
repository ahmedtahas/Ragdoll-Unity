using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chronopen : MonoBehaviour
{
    private Transform body;
    private Vector3 savedPosition;
    private float savedHealth;
    private float health;

    void Start()
    {
        body = transform.Find("Body");
        health = 100f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SaveState();
            Invoke("RestoreState", 3f);
        }
    }

    private void SaveState()
    {
        savedPosition = body.transform.position;
        savedHealth = health;
    }

    private void RestoreState()
    {
        body.transform.position = savedPosition;
        health = savedHealth;
    }
}