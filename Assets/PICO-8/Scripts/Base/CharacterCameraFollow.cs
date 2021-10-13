#define NO_ENABLE_DEBUG
using UnityEngine;

public class CharacterCameraFollow : MonoBehaviour
{
    public Transform PlayerTrans;
    [SerializeField]
    private float _followSpeed = 10.0f;

    private float _boundWidth;
    private float _boundHeight;
    private Vector3 _currentVelocity;
    private Vector3 _originPosition;
    private LineRenderer _lineRenderer;


    private Vector3 CameraTargetPosition
    {
        get
        {
            var cameraPostion = transform.position;
            var targetPosition = PlayerTrans.position;
            float differenceTargetX = cameraPostion.x - targetPosition.x;
            float differenceTargetY = cameraPostion.y - targetPosition.y;
            if (Mathf.Abs(differenceTargetX) > _boundWidth)
            {
                if (differenceTargetX > 0)
                    cameraPostion -= _boundWidth * Vector3.right;
                else if (differenceTargetX < 0)
                    cameraPostion += _boundWidth * Vector3.right;
            }
            if (Mathf.Abs(differenceTargetY) > _boundHeight)
            {
                if (differenceTargetY > 0)
                    cameraPostion -= _boundHeight * Vector3.up;
                else
                    cameraPostion += _boundHeight * Vector3.up;
            }
            return cameraPostion;
        }
    }

    public void Awake()
    {
        GetScreenSize();
        InitLineRenderer();
        _originPosition = transform.position;
    }

    private void GetScreenSize()
    {
        var renderingRate = (float)Display.main.renderingWidth / Display.main.renderingHeight;
        _boundWidth = Camera.main.orthographicSize * renderingRate * 0.309f;
        _boundHeight = Camera.main.orthographicSize * 0.309f;
        //Console.LogFormat("Width {0}, height {1}, Rate {2}", _screenHalfWidth, _screenHalfHeight, renderingRate);
    }

    private void InitLineRenderer()
    {
        var center = transform.position;
        var go = new GameObject("BoundBox");
        go.transform.SetParent(transform);
        go.transform.localPosition = Vector3.zero;
        _lineRenderer = go.AddComponent<LineRenderer>();
        _lineRenderer.startWidth = 0.1f;
        _lineRenderer.endWidth = 0.1f;
    }

    [System.Diagnostics.Conditional("ENABLE_DEBUG")]
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

    public void UpdateFollow(float deltaTime)
    {
        DrawBoundBox();
        //transform.position = Vector3.Lerp(transform.position, CameraTargetPosition, _followSpeed * Time.deltaTime);
        //transform.position = Vector3.Slerp(transform.position, CameraTargetPosition, _followSpeed * Time.deltaTime);
        transform.position = Vector3.SmoothDamp(transform.position, CameraTargetPosition, ref _currentVelocity, deltaTime, _followSpeed);
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void SetOriginPosition()
    {
        transform.position = _originPosition;
    }
}
