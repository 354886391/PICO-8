using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Player : MonoBehaviour
{

    #region Constants

    public const float MaxFall = 16.0f;
    private const float Gravity = 90.0f;
    private const float HalfGravThreshold = 4.0f;

    private const float FastMaxFall = 24.0f;
    private const float FastMaxAccel = 30.0f;

    public const float MaxRun = 9.0f;
    public const float RunAccel = 100.0f;
    private const float RunReduce = 40.0f;
    private const float AirMult = .65f;

    private const float HoldingMaxRun = 7.0f;
    private const float HoldMinTime = .35f;

    private const float BounceAutoJumpTime = .1f;

    private const float DuckFriction = 50.0f;
    private const int DuckCorrectCheck = 4;
    private const float DuckCorrectSlide = 5.0f;

    private const float DodgeSlideSpeedMult = 1.2f;
    private const float DuckSuperJumpXMult = 1.25f;
    private const float DuckSuperJumpYMult = .5f;

    private const float JumpGraceTime = 0.1f;
    private const float JumpSpeed = -10.5f;
    private const float JumpHBoost = 4.0f;
    private const float VarJumpTime = .2f;
    private const float CeilingVarJumpGrace = .05f;
    private const int UpwardCornerCorrection = 4;
    private const float WallSpeedRetentionTime = .06f;

    private const int WallJumpCheckDist = 3;
    private const float WallJumpForceTime = .16f;
    private const float WallJumpHSpeed = MaxRun + JumpHBoost;

    public const float WallSlideStartMax = 2.0f;
    private const float WallSlideTime = 1.2f;

    private const float BounceVarJumpTime = .2f;
    private const float BounceSpeed = -14.0f;
    private const float SuperBounceVarJumpTime = .2f;
    private const float SuperBounceSpeed = -18.5f;

    private const float SuperJumpSpeed = JumpSpeed;
    private const float SuperJumpH = 26.0f;
    private const float SuperWallJumpSpeed = -16.0f;
    private const float SuperWallJumpVarTime = .25f;
    private const float SuperWallJumpForceTime = .2f;
    private const float SuperWallJumpH = MaxRun + JumpHBoost * 2;

    private const float DashSpeed = 24.0f;
    private const float EndDashSpeed = 16.0f;
    private const float EndDashUpMult = .75f;
    private const float DashTime = .15f;
    private const float DashCooldown = .2f;
    private const float DashRefillCooldown = .1f;
    private const int DashHJumpThruNudge = 6;
    private const int DashCornerCorrection = 4;
    private const int DashVFloorSnapDist = 3;
    private const float DashAttackTime = .3f;

    private const float BoostMoveSpeed = 8.0f;
    public const float BoostTime = .25f;

    private const float DuckWindMult = 0f;
    private const int WindWallDistance = 3;

    private const float ReboundSpeedX = 12.0f;
    private const float ReboundSpeedY = -12.0f;
    private const float ReboundVarJumpTime = .15f;

    private const float ReflectBoundSpeed = 22.0f;

    private const float DreamDashSpeed = DashSpeed;
    private const int DreamDashEndWiggle = 5;
    private const float DreamDashMinTime = .1f;

    public const float ClimbMaxStamina = 11.0f;
    private const float ClimbUpCost = 10.0f / 2.2f;
    private const float ClimbStillCost = 10.0f / 10f;
    private const float ClimbJumpCost = 11.0f / 4f;
    private const int ClimbCheckDist = 2;
    private const int ClimbUpCheckDist = 2;
    private const float ClimbNoMoveTime = .1f;
    public const float ClimbTiredThreshold = 2.0f;
    private const float ClimbUpSpeed = -4.5f;
    private const float ClimbDownSpeed = 8.0f;
    private const float ClimbSlipSpeed = 3.0f;
    private const float ClimbAccel = 90.0f;
    private const float ClimbGrabYMult = .2f;
    private const float ClimbHopY = -12.0f;
    private const float ClimbHopX = 10.0f;
    private const float ClimbHopForceTime = .2f;
    private const float ClimbJumpBoostTime = .2f;
    private const float ClimbHopNoWindTime = .3f;

    private const float LaunchSpeed = 28.0f;
    private const float LaunchCancelThreshold = 22.0f;

    private const float LiftYCap = -13.0f;
    private const float LiftXCap = 25.0f;

    private const float JumpThruAssistSpeed = -4.0f;

    public const float WalkSpeed = 6.4f;

    public const int StNormal = 0;
    public const int StClimb = 1;
    public const int StDash = 2;
    public const int StSwim = 3;
    public const int StBoost = 4;
    public const int StRedDash = 5;
    public const int StHitSquash = 6;
    public const int StLaunch = 7;
    public const int StPickup = 8;
    public const int StDreamDash = 9;
    public const int StSummitLaunch = 10;
    public const int StDummy = 11;
    public const int StIntroWalk = 12;
    public const int StIntroJump = 13;
    public const int StIntroRespawn = 14;
    public const int StIntroWakeUp = 15;
    public const int StBirdDashTutorial = 16;
    public const int StFrozen = 17;
    public const int StReflectionFall = 18;
    public const int StStarFly = 19;
    public const int StTempleFall = 20;
    public const int StCassetteFly = 21;
    public const int StAttract = 22;
    #endregion

}
