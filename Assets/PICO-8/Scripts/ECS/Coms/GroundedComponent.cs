using UnityEngine;

public struct RayOrigin
{
    public Vector2 topLeft;
    public Vector2 bottomLeft;
    public Vector2 bottomRight;
}

public class GroundedComponent : IComponent
{
    public float VerticalRaysCount = 5;
    public float HorizontalRaysCount = 5;
    public float VerticalRaysInterval;
    public float HorizontalRaysInterval;

    public RayOrigin RayOrigin;   
    public LayerMask GroundMask;    // 包含头顶和脚下的检测
    public LayerMask CeilingMask;   // 单向平台的检测
    public Rigidbody2D Rigidbody;
    public BoxCollider2D BoxCollider;
    public RaycastHit2D RaycastGround;
    public RaycastHit2D RaycastCeiling;
}
