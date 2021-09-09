using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform _targetTransform;
    [SerializeField]
    private float _followSpeed = 12.0f;

    private float _screenHalfWidth;     // half
    private float _screenHalfHeight;    // half
    private Vector3 currentVelocity;
    private LineRenderer _lineRenderer;

    private Vector3 CameraTargetPosition
    {
        get
        {
            var cameraPostion = transform.position;
            var targetPosition = _targetTransform.position;
            float differenceTarget = cameraPostion.x - targetPosition.x;
            if (Mathf.Abs(differenceTarget) > _screenHalfHeight)
            {
                if (differenceTarget > 0)
                    cameraPostion -= (differenceTarget - _screenHalfHeight) * Vector3.right;
                else if (differenceTarget < 0)
                    cameraPostion -= (differenceTarget + _screenHalfHeight) * Vector3.right;
            }
            return cameraPostion;
        }
    }

    public void Awake()
    {
        GetScreenSize();
        InitLineRenderer();
    }

    private void GetScreenSize()
    {
        var renderingRate = (float)Display.main.renderingWidth / Display.main.renderingHeight;
        _screenHalfHeight = Camera.main.orthographicSize;
        _screenHalfWidth = _screenHalfHeight * renderingRate;
        Console.LogFormat("Width {0}, height {1}, Rate {2}", _screenHalfWidth, _screenHalfHeight, renderingRate);
    }

    private void InitLineRenderer()
    {
        var center = transform.position;
        var go = new GameObject("BoundBox");
        go.transform.SetParent(transform);
        go.transform.position = new Vector3(center.x - _screenHalfWidth, center.y - _screenHalfHeight);
        _lineRenderer = go.AddComponent<LineRenderer>();
        _lineRenderer.startWidth = 0.1f;
        _lineRenderer.endWidth = 0.1f;
    }

    [System.Diagnostics.Conditional("ENABLE_DEBUG")]
    private void DrawBoundBox()
    {
        var center = transform.position;
        var min = new Vector3(center.x - _screenHalfWidth, center.y - _screenHalfHeight);
        var max = new Vector3(center.x + _screenHalfWidth, center.y + _screenHalfHeight);
        _lineRenderer.positionCount = 5;
        _lineRenderer.SetPosition(0, new Vector3(max.x, min.y, -0.1f));
        _lineRenderer.SetPosition(1, new Vector3(max.x, max.y, -0.1f));
        _lineRenderer.SetPosition(2, new Vector3(min.x, max.y, -0.1f));
        _lineRenderer.SetPosition(3, new Vector3(min.x, min.y, -0.1f));
        _lineRenderer.SetPosition(4, new Vector3(max.x, min.y, -0.1f));
    }

    public void LateUpdate()
    {
        DrawBoundBox();
        transform.position = Vector3.SmoothDamp(transform.position, CameraTargetPosition, ref currentVelocity, Time.deltaTime, _followSpeed);
    }
}
