using UnityEngine;

public class RaycastComponent : IComponent
{
    public float VRaysCount = 5;
    public float HRaysCount = 5;
    public float VRaysInterval;
    public float HRaysInterval;

    public Bounds Origin;       // 角色边框坐标
    //public LayerMask GroundMask;    // 单项平台的检测
    //public LayerMask PlatformMask;   // 双向平台的检测
    //public Rigidbody2D Rigidbody;
    //public BoxCollider2D BoxCollider;
    public RaycastHit2D RaycastGround { get; set; }
    public RaycastHit2D RaycastWall { get; set; }
}
