using UnityEngine;

public enum Facings
{
    Left = -1,
    Right = 1,
}

public enum States
{
    None,
    Idle,
    Move,
    Jump,
    Dash,
    Climb,
    WallClimb,
}

public struct Bounds
{
    public Vector2 topLeft;
    public Vector2 bottomLeft;
    public Vector2 bottomRight;
}

public class StateComponent : IComponent
{

    public float VRaysCount = 5;
    public float HRaysCount = 5;
    public float VRaysInterval;
    public float HRaysInterval;

    public bool CanMove { get; set; }
    public bool OnGround { get; set; }
    public bool WasOnGround { get; set; }    // 上一帧是否在地面
    public bool AgainstWall { get; set; }    // 当前帧是否贴墙壁

    public Vector2 Speed;                    // 移动速度
    public Bounds Origin;                 // 角色边框坐标
    public States State { get; set; }        // 当前状态         
    public Facings Facing { get; set; }      // 角色面部朝向
    public bool IsRising { get { return !OnGround && Speed.y > Constants.MinOffset; } }
    public bool IsFalling { get { return !OnGround && Speed.y < Constants.MinOffset; } }  
    //public Vector2 Movement;               // 位移
}
