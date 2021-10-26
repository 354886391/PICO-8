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

[System.Serializable]
public class StateComponent : IComponent
{

    public bool CanMove;
    public bool OnGround;
    public bool WasOnGround;    // 上一帧是否在地面
    public bool AgainstWall;    // 当前帧是否贴墙壁

    public Bounds Origin;                   // 角色边框坐标
    public States State;                    // 当前状态         
    public Facings Facing;                  // 角色面部朝向
    public Vector2 Speed;       // 移动速度
    public Rigidbody2D Rigidbody;
    public BoxCollider2D BoxCollider;
    public bool IsRising { get { return !OnGround && Speed.y > Constants.MinOffset; } }
    public bool IsFalling { get { return !OnGround && Speed.y < Constants.MinOffset; } }
    //public Vector2 Movement;               // 位移
}
