using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MultiTargetCamera : MonoBehaviour
{
    private float minZoom = 7f;
    private float maxZoom = 95f;
    private float zoomLimiter = 180f;
    private float zoomSpeed = 0.1f;
    private Camera cam;
    private List<Transform> gameObjectsInView = new List<Transform>();
    private List<Transform> gameObjectsInBlackout = new List<Transform>();
    private bool isBlackout = false;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void OnEnable()
    {
        StartCoroutine(GameReady());
    }

    IEnumerator GameReady()
    {
        yield return new WaitUntil(() => GameManager.Instance != null);
        GameManager.Instance.OnBlindEnemy += Blackout;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnBlindEnemy -= Blackout;
    }

    private void Update()
    {
        Move();
        Zoom();
    }

    public void AddToView(Transform transform)
    {
        if (gameObjectsInView.Contains(transform)) return;
        if (isBlackout) gameObjectsInBlackout.Add(transform);
        else gameObjectsInView.Add(transform);
    }

    public void RemoveFromView(Transform transform)
    {
        if (gameObjectsInView.Contains(transform)) gameObjectsInView.Remove(transform);
    }

    public void Blackout(float duration, GameObject caller = null)
    {
        if (GameManager.Instance.gameMode == Constants.SINGLE_PLAYER) return;
        StartCoroutine(BlackoutRoutine(duration));
    }

    public IEnumerator BlackoutRoutine(float duration = 10)
    {
        isBlackout = true;
        gameObjectsInBlackout.AddRange(gameObjectsInView);
        gameObjectsInView.Clear();
        gameObjectsInView.Add(GameManager.Instance.player.transform);
        yield return new WaitForSeconds(duration);
        isBlackout = false;
        gameObjectsInView.Clear();
        foreach (Transform t in gameObjectsInBlackout)
        {
            if (t != null)
            {
                gameObjectsInView.Add(t);
            }
        }
        gameObjectsInBlackout.Clear();
    }

    private void Move()
    {
        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = new Vector3(centerPoint.x, centerPoint.y, -1);

        transform.position = newPosition;
    }

    private void Zoom()
    {
        float greatestDistance = GetGreatestDistance();
        float newZoom = Mathf.Lerp(minZoom, maxZoom, greatestDistance / zoomLimiter);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, zoomSpeed);
    }

    private Vector3 GetCenterPoint()
    {
        if (gameObjectsInView.Count == 0)
            return Vector3.zero;

        Bounds bounds = new Bounds(gameObjectsInView[0].position, Vector3.zero);
        for (int i = 0; i < gameObjectsInView.Count; i++)
        {
            bounds.Encapsulate(gameObjectsInView[i].position);
        }
        return bounds.center;
    }

    private float GetGreatestDistance()
    {
        if (gameObjectsInView.Count <= 1)
            return 0f;

        float minX = gameObjectsInView[0].position.x;
        float maxX = gameObjectsInView[0].position.x;
        float minY = gameObjectsInView[0].position.y;
        float maxY = gameObjectsInView[0].position.y;

        for (int i = 1; i < gameObjectsInView.Count; i++)
        {
            if (gameObjectsInView[i].position.x < minX)
                minX = gameObjectsInView[i].position.x;
            else if (gameObjectsInView[i].position.x > maxX)
                maxX = gameObjectsInView[i].position.x;

            if (gameObjectsInView[i].position.y < minY)
                minY = gameObjectsInView[i].position.y;
            else if (gameObjectsInView[i].position.y > maxY)
                maxY = gameObjectsInView[i].position.y;
        }

        float width = maxX - minX;
        float height = maxY - minY;

        return Mathf.Max(width, height);
    }
}
