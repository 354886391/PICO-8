#define ENABLE_DEBUG
using UnityEngine;

/// <summary>
/// ��ɫ��ײ
/// </summary>
public class BodyDetection : MonoBehaviour
{

    public const float _skinWidth = 0.02f;  // Ƥ�����

    private float _rayHSpacing;             // ˮƽ���߼��
    private float _rayVSpacing;             // ��ֱ���߼��  
    private Origin2D _rayOrigin;        // ������ʼ��

    private int _ignoreLayer = 2;       // ���Բ�
    private int _cachedLayer;           // 
    private LayerMask _detectLayer;     // ����

    public int rayCount = 8;
    public CollisionInfo wallHit;
    public CollisionInfo groundHit;

    private Collider2D _bodyCollider;   // ��ɫ��ײ��

    private void Awake()
    {
        _bodyCollider = GetComponent<Collider2D>();
        _detectLayer = LayerMask.GetMask("Ground");
    }

    private void Start()
    {
        ComputeRayOrigin();             // �����������
        ComputeRaySpacing(rayCount - 1);    // �������߼��
    }

    /// <summary>
    /// �����ײ
    /// </summary>
    /// <param name="movement">�޸�������߳���</param>
    public void DetectRaycast(ref Vector2 movement)
    {
        ResetHitInfo();
        ComputeRayOrigin();
        DisableRaycastLayer();
        HorizontalHit(ref movement.x);
        VerticalHit(ref movement.y);
        EnableRaycastLayer();
    }

    /// <summary>
    /// ���߳��Ȳ�����Ƥ�����
    /// </summary>
    public bool Raycast(Vector2 origin, Vector2 direction, out RaycastHit2D hitInfo, float distance, LayerMask mask)
    {
        hitInfo = Physics2D.Raycast(origin, direction, distance, mask);
        //if (hitInfo) hitInfo.distance -= _skinWidth;
        GameUtility.DebugRay(origin, direction * distance, Color.red);
        return hitInfo;
    }

    /// <summary>
    /// ���ˮƽ��ײ
    /// </summary>
    public bool HorizontalHit(ref float movementX)
    {
        if (movementX == 0) return false;
        var isRight = movementX > MathEx.MIN;
        var origin = isRight ? _rayOrigin.right : _rayOrigin.left;
        var direction = isRight ? Vector2.right : Vector2.left;
        var distance = Mathf.Abs(movementX) + _skinWidth;
        for (int i = 0; i < rayCount; i++)
        {
            var rayOrigin = new Vector2(origin.x, origin.y + _rayVSpacing * i);
            var raycastHit = Raycast(rayOrigin, direction, out RaycastHit2D hitInfo, distance, _detectLayer);
            if (raycastHit)
            {
                if (isRight)
                {
                    movementX = hitInfo.distance - _skinWidth;
                }
                else
                {
                    movementX = -hitInfo.distance + _skinWidth;
                }
                wallHit.SetFrom(hitInfo);
                wallHit.isHit = Mathf.Abs(movementX) < MathEx.MIN;
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// ���������ײ
    /// </summary>
    public bool VerticalHit(ref float movementY)
    {
        if (movementY == 0) return false;
        var isUpward = movementY > MathEx.MIN;
        var origin = isUpward ? _rayOrigin.top : _rayOrigin.left;
        var direction = isUpward ? Vector2.up : Vector2.down;
        var distance = Mathf.Abs(movementY) + _skinWidth;
        for (int i = 0; i < rayCount; i++)
        {
            var rayOrigin = new Vector2(origin.x + _rayHSpacing * i, origin.y);
            var raycastHit = Raycast(rayOrigin, direction, out RaycastHit2D hitInfo, distance, _detectLayer);
            if (raycastHit)
            {
                if (isUpward)
                {
                    movementY = hitInfo.distance - _skinWidth;
                }
                else
                {
                    movementY = -hitInfo.distance + _skinWidth;
                }
                groundHit.SetFrom(hitInfo);
                groundHit.isHit = Mathf.Abs(movementY) < MathEx.MIN;
                return true;
            }
        }
        return false;
    }

    public void ResetHitInfo()
    {
        wallHit.Clear();
        groundHit.Clear();
    }

    public void DisableRaycastLayer()
    {
        _cachedLayer = gameObject.layer;
        gameObject.layer = _ignoreLayer;
    }

    public void EnableRaycastLayer()
    {
        gameObject.layer = _cachedLayer;
    }

    private void ComputeRaySpacing(float rayCount)
    {
        _rayHSpacing = (_rayOrigin.right.x - _rayOrigin.left.x) / rayCount;
        _rayVSpacing = (_rayOrigin.top.y - _rayOrigin.left.y) / rayCount;
    }

    private void ComputeRayOrigin()
    {
        var bounds = _bodyCollider.bounds;
        bounds.Expand(_skinWidth * -2f);
        _rayOrigin.top = new Vector2(bounds.min.x, bounds.max.y);
        _rayOrigin.left = new Vector2(bounds.min.x, bounds.min.y);
        _rayOrigin.right = new Vector2(bounds.max.x, bounds.min.y);
        GameUtility.DebugCube(bounds.min, bounds.max, Color.yellow);
    }

    #region Test
    private void OnDrawGizmosSelected()
    {
        DrawBounds(Color.yellow);
    }

    private void DrawBounds(Color color)
    {
        if (!_bodyCollider) return;
        var bounds = _bodyCollider.bounds;
        bounds.Expand(_skinWidth * -2f);
        Gizmos.color = color;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
    #endregion
}

#region STRUCT
/// <summary>
/// �������
/// </summary>
[System.Serializable]
public struct Origin2D
{
    public Vector2 top;     // topLeft
    public Vector2 left;    // bottomLeft;
    public Vector2 right;   // bottomRight

    public Origin2D(Vector2 start, Vector2 direction) : this()
    {
        left = start;
        right = direction;
    }
}


/// <summary>
/// ��ײ��Ϣ
/// </summary>
[System.Serializable]
public struct CollisionInfo
{

    public bool isHit;
    //public bool isValid;

    public Vector2 point;
    public Vector2 normal;
    public float distance;
    public Collider2D collider;
    public Rigidbody2D rigidbody;

    public CollisionInfo(CollisionInfo other) : this()
    {
        isHit = other.isHit;
        //isValid = other.isValid;

        point = other.point;
        normal = other.normal;
        distance = other.distance;
        collider = other.collider;
        rigidbody = other.rigidbody;
    }

    public CollisionInfo(RaycastHit2D hitInfo) : this()
    {
        SetFrom(hitInfo);
    }

    public void SetFrom(RaycastHit2D hitInfo)
    {
        point = hitInfo.point;
        normal = hitInfo.normal;
        distance = hitInfo.distance;
        collider = hitInfo.collider;
        rigidbody = hitInfo.rigidbody;
    }

    public void Clear()
    {
        isHit = false;
        //isValid = false;

        distance = 0;
        collider = null;
        rigidbody = null;
        point = Vector2.zero;
        normal = Vector2.zero;
    }
}

#endregion