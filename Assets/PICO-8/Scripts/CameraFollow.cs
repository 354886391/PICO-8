using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform _targetTransform;
    [SerializeField]
    private float _followSpeed = 3.0f;
    [SerializeField]
    private float distanceToTarget;

    private float _boundWidth;  //half
    private float _boundHeight; //half
    private LineRenderer _lineRenderer;

    private Vector3 CameraTargetPosition
    {
        get
        {
            var cameraTargetPostion = transform.position;
            float differenceTarget = transform.position.x - _targetTransform.position.x;

            if (differenceTarget > 0)   // target is to the left of bound
            {
                if (Mathf.Abs(differenceTarget) > _boundWidth)
                {
                    cameraTargetPostion = transform.position - (differenceTarget - _boundHeight) * Vector3.right;
                }
            }
            else if (differenceTarget < 0)  // right
            {
                if (Mathf.Abs(differenceTarget) > _boundWidth)
                {
                    cameraTargetPostion = transform.position - (differenceTarget + _boundWidth) * Vector3.right;
                }
            }
            return cameraTargetPostion;
        }
    }

    private Vector3 CameraTargetPosition2
    {
        get
        {
            var wannaPosition = transform.position;
            var cameraPostion = transform.position;
            var targetPosition = _targetTransform.position;

            var difference = targetPosition - cameraPostion;
            if (difference.x < 0)
            {
                wannaPosition = difference - _boundWidth * Vector3.left;
            }
            if (difference.x > 0)
            {
                wannaPosition = difference - _boundWidth * Vector3.right;
            }
            return wannaPosition;
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
        var cameraHeight = Camera.main.orthographicSize;
        var cameraWidth = cameraHeight * renderingRate;
        _boundWidth = cameraWidth * 0.618f;
        _boundHeight = cameraHeight * 0.618f;
        Console.LogFormat("Width {0}, height {1}, Rate {2}", cameraWidth, cameraHeight, renderingRate);
    }

    private void InitLineRenderer()
    {
        var center = transform.position;
        var go = new GameObject("BoundBox");
        go.transform.SetParent(transform);
        go.transform.position = new Vector3(center.x - _boundWidth, center.y - _boundHeight);
        _lineRenderer = go.AddComponent<LineRenderer>();
        _lineRenderer.startWidth = 0.1f;
        _lineRenderer.endWidth = 0.1f;
    }

    private void DrawBoundBox()
    {
        var center = transform.position;
        var min = new Vector3(center.x - _boundWidth, center.y - _boundHeight);
        var max = new Vector3(center.x + _boundWidth, center.y + _boundHeight);
        _lineRenderer.positionCount = 5;
        _lineRenderer.SetPosition(0, new Vector3(max.x, min.y, -0.1f));
        _lineRenderer.SetPosition(1, new Vector3(max.x, max.y, -0.1f));
        _lineRenderer.SetPosition(2, new Vector3(min.x, max.y, -0.1f));
        _lineRenderer.SetPosition(3, new Vector3(min.x, min.y, -0.1f));
        _lineRenderer.SetPosition(4, new Vector3(max.x, min.y, -0.1f));
    }

    public void Update()
    {
        DrawBoundBox();
    }

    public void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, CameraTargetPosition, _followSpeed * Time.deltaTime);
    }
}
