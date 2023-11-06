using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTargetCamera : MonoBehaviour
{
    public float minZoom = 5f;
    public float maxZoom = 75f;
    public float zoomLimiter = 140f;
    public float zoomSpeed = 0.1f;

    private Camera cam;
    private List<Transform> gameObjectsInView = new List<Transform>();

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        Move();
        Zoom();
    }

    public void AddToView(Transform transform)
    {
        gameObjectsInView.Add(transform);
    }

    public void RemoveFromView(Transform transform)
    {
        gameObjectsInView.Remove(transform);
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
