using UnityEngine.Events;

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


    public bool IsClimbing { get; set; }
    public bool CanClimb { get; set; }
    public bool CanUpdate { get; set; }
    public bool IsCooldown { get; set; }

    public float ClimbTimer { get; set; }
    public float HeldDownTimer { get; set; }
    public float CooldownTimer { get; set; }
    public float WallSlideTimer { get; set; }
    public float NoMoveTimer { get; set; }

    public UnityAction BeginEvent { get; set; }
    public UnityAction UpdateEvent { get; set; }
    public UnityAction EndEvent { get; set; }
    public UnityAction LandEvent { get; set; }

}
