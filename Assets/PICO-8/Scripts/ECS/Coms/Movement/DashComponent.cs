using UnityEngine;

public class DashComponent : IComponent
{
    public bool Dash;
    public bool IsDashing;
    public bool CanDash;
    public bool CanDashUpdate;
    public bool IsDashCooldown;

    public float DashTimer;
    public float DashHeldDownTimer;
    public float DashCooldownTimer;
    public Vector2 DashDir;
    public Vector2 BeforeDashSpeed;
}
