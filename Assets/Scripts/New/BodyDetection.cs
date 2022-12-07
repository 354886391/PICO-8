#define ENABLE_DEBUG
using UnityEngine;

/// <summary>
/// 角色碰撞
/// </summary>
public class BodyDetection : MonoBehaviour
{
    private const int _rayCount = 8;
    private const float _skinWidth = 0.02f;

    private float _rayHGap;             // 水平射线间距
    private float _rayVGap;             // 竖直射线间距  
    private Origin2D _rayOrigin;        // 射线起始点

    private int _cachedLayer;           //
    private int _ignoreLayer = 2;       // 忽略层
    private LayerMask _layerMask;       // 检测层

    private Collider2D _bodyCollider;   // 角色碰撞体

    public CollisionInfo wallHit;
    public CollisionInfo groundHit;


    private void Awake()
    {
        _bodyCollider = GetComponent<Collider2D>();
        _layerMask = LayerMask.GetMask("Ground");
    }

    private void Start()
    {
        ComputeRayOrigin();
        ComputeRayGap(_rayCount - 1);
    }

    public void DetectRaycast(ref Vector2 movement)
    {
        ResetHitInfo();
        DisableRaycastLayer();
        ComputeRayOrigin();
        HorizontalHit(ref movement.x);
        VerticalHit(ref movement.y);
        EnableRaycastLayer();
    }

    #region PUBLIC


    /// <summary>
    /// 检测左右碰撞
    /// 修改射线长度
    /// </summary>
    public bool HorizontalHit(ref float movementX)
    {
        if (movementX == 0) return false;
        var isRight = movementX > MathEx.MIN;
        var origin = isRight ? _rayOrigin.right : _rayOrigin.left;
        var direction = isRight ? Vector2.right : Vector2.left;
        var distance = Mathf.Abs(movementX) + 2 * _skinWidth;
        for (int i = 0; i < _rayCount; i++)
        {
            var rayOrigin = new Vector2(origin.x, origin.y + _rayVGap * i);
            var raycastHit = Raycast(rayOrigin, direction, out RaycastHit2D hitInfo, distance, _layerMask);
            if (raycastHit)
            {
                if (isRight)
                {
                    movementX = hitInfo.distance;
                }
                else
                {
                    movementX = -hitInfo.distance;
                }
                wallHit.SetFrom(hitInfo);
                wallHit.isHit = Mathf.Abs(movementX) < MathEx.MIN;
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 检测上下碰撞
    /// 修改射线长度
    /// </summary>
    public bool VerticalHit(ref float movementY)
    {
        if (movementY == 0) return false;
        var isUpward = movementY > MathEx.MIN;
        var origin = isUpward ? _rayOrigin.top : _rayOrigin.left;
        var direction = isUpward ? Vector2.up : Vector2.down;
        var distance = Mathf.Abs(movementY) + 2 * _skinWidth;
        for (int i = 0; i < _rayCount; i++)
        {
            var rayOrigin = new Vector2(origin.x + _rayHGap * i, origin.y);
            var raycastHit = Raycast(rayOrigin, direction, out RaycastHit2D hitInfo, distance, _layerMask);
            if (raycastHit)
            {
                if (isUpward)
                {
                    movementY = hitInfo.distance;
                }
                else
                {
                    movementY = -hitInfo.distance;
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

    /// <summary>
    /// 射线长度不包含皮肤厚度
    /// </summary>
    public bool Raycast(Vector2 origin, Vector2 direction, out RaycastHit2D hitInfo, float distance, LayerMask mask)
    {
        hitInfo = Physics2D.Raycast(origin, direction, distance, mask);
        if (hitInfo) hitInfo.distance -= _skinWidth;
        GameUtility.DebugRay(origin, direction * distance, Color.red);
        return hitInfo;
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
    #endregion

    private void ComputeRayGap(float rayCount)
    {
        _rayHGap = (_rayOrigin.right.x - _rayOrigin.left.x) / rayCount;
        _rayVGap = (_rayOrigin.top.y - _rayOrigin.left.y) / rayCount;
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
}

#region STRUCT
/// <summary>
/// 射线起点
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

[System.Serializable]
public struct CollisionInfo
{

    public bool isHit;
    public bool isValid;

    public Vector2 point;
    public Vector2 normal;
    public float distance;
    public Collider2D collider;
    public Rigidbody2D rigidbody;

    public CollisionInfo(CollisionInfo other) : this()
    {
        isHit = other.isHit;
        isValid = other.isValid;

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
        isValid = false;

        point = Vector2.zero;
        normal = Vector2.zero;
        distance = 0;
        collider = null;
        rigidbody = null;
    }
}

#endregion