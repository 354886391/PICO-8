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

public class StateComponent : IComponent
{
    public bool CanMove;
    public bool OnGround;       // 当前帧是否在地面
    public bool WasOnGround;    // 上一帧是否在地面
    public bool AgainstWall;    // 当前帧是否贴墙壁

    public States State;        // 当前状态
    public Vector2 Speed;       // 移动速度   
    public Facings Facing;      // 角色面部朝向
    public Vector2 Movement;    // 位移
}
