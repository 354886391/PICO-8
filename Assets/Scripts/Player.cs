using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    #region Move
    public readonly float MaxRun = 9f;
    public readonly float MaxFall = -16f;
    public readonly float FallAccel = 30f;
    public readonly float RunAccel = 100f;
    public readonly float RunReduce = 40f;
    public readonly float Gravity = 90f;
    public readonly float HalfGravThreshold = 4f;
    public readonly float GroundMult = 1.0f;
    public readonly float AirMult = 0.65f;
    #endregion

    #region Jump
    public readonly float JumpHBoost = 4f;
    public readonly float JumpWBoost = 6f;
    public readonly float JumpSpeed = 10.5f;
    public readonly float JumpTime = 0.2f;
    public readonly float JumpToleranceTime = 0.15f;
    #endregion

    #region Dash
    public readonly float DashSpeed = 24f;
    public readonly float DashTime = 0.15f;
    public readonly float DashToleranceTime = 0.15f;
    public readonly float DashCooldownTime = 0.2f;
    #endregion

    #region Climb
    public readonly float ClimbUpSpeed = 4.5f;
    public readonly float ClimbDownSpeed = -6f;
    public readonly float ClimbSlipSpeed = -3f;
    public readonly float ClimbTime = 2.0f;
    public readonly float ClimbToleranceTime = 0.15f;
    public readonly float ClimbCooldownTime = 0.2f;
    public readonly float ClimbAccel = 90f;
    public readonly float ClimbSlideAccel = 90f;
    public readonly float ClimbGrabYMult = .2f;
    #endregion
}
