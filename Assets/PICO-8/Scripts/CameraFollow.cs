using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform _targetTransform;
    [SerializeField]
    private float _followSpeed = 3.0f;

    public void Awake()
    {
        GetScreenSize();
    }

    private float _boundWidth;
    private float _boundHeight;

    private void GetScreenSize()
    {
        var displayMain = Display.main;
        var renderingRate = displayMain.renderingWidth / displayMain.renderingHeight;
        var cameraHeight = Camera.main.orthographicSize;
        var cameraWidth = cameraHeight * renderingRate;
        _boundWidth = cameraWidth * 0.618f;
        _boundHeight = cameraHeight * 0.618f;
        Console.LogFormat("Width {0}, height {1}", cameraWidth, cameraHeight);
    }

    private void ComputeTargetPosition()
    {
        var hDistance = transform.position.x - _targetTransform.position.x;
        var vDistance = transform.position.y - _targetTransform.position.y;
        Console.LogFormat("HDistance {0}, VDistance {1} ", hDistance, vDistance);
        var targetPosition = Vector3.zero;
        if (hDistance > _boundWidth)
        {
            targetPosition.x = Mathf.Lerp(targetPosition.x, hDistance, _followSpeed * Time.deltaTime);
        }
        if (vDistance > _boundHeight)
        {
            targetPosition.y = Mathf.Lerp(targetPosition.y, vDistance, _followSpeed * Time.deltaTime);
        }
        targetPosition.z = -50f;
        transform.position = targetPosition;
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


    public void LateUpdate()
    {
        ComputeTargetPosition();
    }
}
