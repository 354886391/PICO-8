public class JumpComponent : IComponent
{
    public bool Jump;
    public bool IsJumping;
    public bool CanJump;
    public bool CanJumpUpdate;

    public int MidAirJumps;
    public float JumpTimer;
    public float JumpHeldDownTimer;
}
