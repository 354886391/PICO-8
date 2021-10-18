public class ClimbComponent : IComponent
{
    public bool Climb;
    public bool IsClimbing;
    public bool CanClimb;
    public bool CanClimbUpdate;
    public bool IsClimbCooldown;

    public float ClimbTimer;
    public float ClimbHeldDownTimer;
    public float ClimbCooldownTimer;
    public float WallSlideTimer;
    public float ClimbNoMoveTimer;
 
}
