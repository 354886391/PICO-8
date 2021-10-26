
[System.Serializable]
public class ClimbComponent : IComponent
{
    public readonly float UpSpeed = 4.5f;
    public readonly float DownSpeed = -6f;
    public readonly float SlipSpeed = -3f;
    public readonly float ClimbTime = 2.0f;
    public readonly float ToleranceTime = 0.15f;
    public readonly float CooldownTime = 0.2f;
    public readonly float ClimbAccel = 90f;
    public readonly float SlideAccel = 90f;
    public readonly float GrabYMult = .2f;


    public bool IsClimbing;
    public bool CanUpdate;
    public bool IsCooldown;

    public float ClimbTimer;
    public float HeldDownTimer;
    public float CooldownTimer;
    public float WallSlideTimer;
    public float NoMoveTimer;

    public System.Action BeginEvent;
    public System.Action UpdateEvent;
    public System.Action EndEvent;
    public System.Action LandEvent;

}
