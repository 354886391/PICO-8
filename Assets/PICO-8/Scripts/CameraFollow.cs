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

    private float _boundWidth;
    private float _boundHeight;

    public void Awake()
    {
        GetScreenSize();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Console.LogFormat("OnTriggerEnter2D {0}", collision.name);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Console.LogFormat("OnTriggerStay2D {0}", collision.name);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Console.LogFormat("OnTriggerExit2D {0}", collision.name);
    }

    private void ComputeDistance()
    {
        float hDistance = Mathf.Abs(transform.position.x - _targetTransform.position.x);
        if (hDistance - _boundHeight > 0)
        {

        }
    }

    public void LateUpdate()
    {

    }
}
