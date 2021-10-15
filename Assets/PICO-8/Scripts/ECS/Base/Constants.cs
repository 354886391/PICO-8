class Constants
{
    #region Move
    public const float MaxRun = 9f;
    public const float MaxFall = -16f;
    public const float FallAccel = 30f;
    public const float RunAccel = 100f;
    public const float RunReduce = 40f;

    public const float Gravity = 90f;
    public const float HalfGravThreshold = 4f;
    public const float AirMult = 0.65f;
    #endregion

    #region Jump
    public const float JumpHBoost = 4f;
    public const float JumpWBoost = 6f;
    public const float JumpSpeed = 10.5f;
    public const float JumpTime = 0.2f;
    public const float JumpToleranceTime = 0.15f;

    public const int MaxMidAirJump = 0;   // 0 or 1
    #endregion

    #region Dash  
    public const float DashSpeed = 24f;
    public const float EndDashSpeed = 16f;
    public const float DashTime = 0.15f;
    public const float DashToleranceTime = 0.15f;
    public const float DashCooldownTime = 0.2f;
    #endregion

    #region Climb
    private const float ClimbUpSpeed = 4.5f;
    private const float ClimbDownSpeed = -6f;
    private const float ClimbSlipSpeed = -3f;
    private const float ClimbTime = 2.0f;
    private const float ClimbToleranceTime = 0.15f;
    private const float ClimbCooldownTime = 0.2f;
    private const float ClimbAccel = 90f;
    private const float SlideAccel = 90f;
    private const float ClimbGrabYMult = .2f;
    #endregion

    public const float SkinWidth = 0.02f;
    public const float MinOffset = 0.0001f;
}
