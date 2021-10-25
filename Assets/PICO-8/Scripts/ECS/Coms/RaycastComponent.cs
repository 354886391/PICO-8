using UnityEngine;

public struct RayOrigin
{
    public Vector2 topLeft;
    public Vector2 bottomLeft;
    public Vector2 bottomRight;
}

public class RaycastComponent : IComponent
{
    public float VRaysCount = 5;
    public float HRaysCount = 5;
    public float VRaysInterval;
    public float HRaysInterval;

    public RayOrigin RayOrigin;
    public LayerMask GroundMask;    // 单项平台的检测
    public LayerMask PlatformMask;   // 双向平台的检测
    public Rigidbody2D Rigidbody;
    public BoxCollider2D BoxCollider;
    public RaycastHit2D RaycastGround;
    public RaycastHit2D RaycastWall;
}
