#define ENABLE_DEBUG
using System;
using UnityEngine;

/// <summary>
/// 角色碰撞器
/// </summary>
public class BodyCollider2D : MonoBehaviour
{

    private float _rayHSpacing;         // 水平射线间距
    private float _rayVSpacing;         // 竖直射线间距  
    private Origin2D _rayOrigin;        // 射线起始点

    private int _ignoreLayer = 2;       // ignoreRaycast Layer
    private int _cachedLayer;           // gameObject Layer

    private Collider2D _bodyCollider;   // 角色触发器

    public int rayCount;
    public float skinWidth;     // 皮肤厚度

    public LayerMask detectLayer;       // 检测层
    public CollisionData horizontal;
    public CollisionData vertical;

    public Action<CollisionData> onCollideH;
    public Action<CollisionData> onCollideV;

    private void Awake()
    {
        rayCount = 8;
        skinWidth = 0.02f;
        detectLayer = LayerMask.GetMask("Ground");

        _bodyCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        ComputeRayOrigin();             // 计算射线起点
        ComputeRaySpacing(rayCount - 1);    // 计算射线间距
    }

    /// <summary>
    /// 检测碰撞并修正射线最大长度
    /// </summary>
    public void DetectRaycast(ref Vector2 vec)
    {
        ResetHitInfo();
        ComputeRayOrigin();
        DisableRaycastLayer();
        HorizontalHit(ref vec.x);
        VerticalHit(ref vec.y);
        EnableRaycastLayer();
    }

    public bool Raycast(Vector2 origin, Vector2 direction, out RaycastHit2D hitInfo, float distance, LayerMask mask)
    {
        hitInfo = Physics2D.Raycast(origin, direction, distance, mask);
        GameUtility.DebugRay(origin, direction * distance, Color.red);
        return hitInfo;
    }

    /// <summary>
    /// 检测水平碰撞
    /// </summary>
    /// <param name="x">射线长度</param>
    /// <returns>是否发生碰撞</returns>
    public bool HorizontalHit(ref float x)
    {
        if (x == 0) return false;
        var isRight = x > MathEx.MIN;
        var origin = isRight ? _rayOrigin.right : _rayOrigin.left;
        var direction = isRight ? Vector2.right : Vector2.left;
        var distance = Mathf.Abs(x) + skinWidth;
        for (int i = 0; i < rayCount; i++)
        {
            var rayOrigin = new Vector2(origin.x, origin.y + _rayVSpacing * i);
            var raycastHit = Raycast(rayOrigin, direction, out RaycastHit2D hitInfo, distance, detectLayer);
            if (raycastHit)
            {
                if (isRight)
                {
                    x = hitInfo.distance - skinWidth;
                }
                else
                {
                    x = -hitInfo.distance + skinWidth;
                }
                horizontal.SetFrom(hitInfo);
                horizontal.isTouch = Mathf.Abs(x) < MathEx.MIN;
                if (horizontal.isTouch) onCollideH(horizontal);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 检测垂直碰撞
    /// </summary>
    /// <param name="y">射线长度</param>
    /// <returns>是否发生碰撞</returns>
    public bool VerticalHit(ref float y)
    {
        if (y == 0) return false;
        var isUpward = y > MathEx.MIN;
        var origin = isUpward ? _rayOrigin.top : _rayOrigin.left;
        var direction = isUpward ? Vector2.up : Vector2.down;
        var distance = Mathf.Abs(y) + skinWidth;
        for (int i = 0; i < rayCount; i++)
        {
            var rayOrigin = new Vector2(origin.x + _rayHSpacing * i, origin.y);
            var raycastHit = Raycast(rayOrigin, direction, out RaycastHit2D hitInfo, distance, detectLayer);
            if (raycastHit)
            {
                if (isUpward)
                {
                    y = hitInfo.distance - skinWidth;
                }
                else
                {
                    y = -hitInfo.distance + skinWidth;
                }
                vertical.SetFrom(hitInfo);
                vertical.isTouch = Mathf.Abs(y) < MathEx.MIN;
                if (vertical.isTouch) onCollideV(vertical);
                return true;
            }
        }
        return false;
    }

    public void ResetHitInfo()
    {
        horizontal.Clear();
        vertical.Clear();
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
        bounds.Expand(skinWidth * -2f);
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
        bounds.Expand(skinWidth * -2f);
        Gizmos.color = color;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
    #endregion
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


/// <summary>
/// 碰撞信息
/// </summary>
[System.Serializable]
public struct CollisionData
{

    public bool isTouch;
    //public bool isValid;

    public Vector2 point;
    public Vector2 normal;
    public float distance;
    public Collider2D collider;
    public Rigidbody2D rigidbody;

    public CollisionData(CollisionData other) : this()
    {
        isTouch = other.isTouch;
        //isValid = other.isValid;

        point = other.point;
        normal = other.normal;
        distance = other.distance;
        collider = other.collider;
        rigidbody = other.rigidbody;
    }

    public CollisionData(RaycastHit2D hitInfo) : this()
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
        isTouch = false;
        //isValid = false;

        point = Vector2.zero;
        normal = Vector2.zero;
        distance = 0;
        collider = null;
        rigidbody = null;
    }
}

#endregion