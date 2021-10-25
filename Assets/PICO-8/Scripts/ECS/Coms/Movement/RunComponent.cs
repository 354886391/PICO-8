public class RunComponent : IComponent
{
    public readonly float MaxRun = 9f;
    public readonly float MaxFall = -16f;
    public readonly float FallAccel = 30f;
    public readonly float RunAccel = 100f;
    public readonly float RunReduce = 40f;
    public readonly float Gravity = 90f;
    public readonly float HalfGravThreshold = 4f;
    public readonly float GroundMult = 1.0f;
    public readonly float AirMult = 0.65f;
}
