#define NO_ENABLE_DEBUG
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    public CharacterHealth Health;
    [SerializeField]
    public CharacterMovement Movement;
    [SerializeField]
    public CharacterAnimation Animation;
    [SerializeField]
    public CharacterHairFlow HairFlow;
    [SerializeField]
    public CharacterCameraFollow CameraFollow;

    private void Awake()
    {
        Time.fixedDeltaTime = 0.02f;
        Application.targetFrameRate = 60;     
    }

    private void Start()
    {
        Health.Initialize(OnDeathbed);
    }

    private void Restart()
    {
        Movement.AutoCanMove(0.2f);
        HairFlow.AutoHideHairFlow(0.21f);
        Utility.DelayCall(0.20f, () =>
        {          
            Movement.SetOriginPosition();
            Movement.BornAndJump();
            Health.Restart();
        });
    }

    private void OnDeathbed()
    {
        Restart();
    }

    private void Update()
    {
        Movement.UpdateInput();
        Animation.UpdateAnimation(this);
        HairFlow.UpdateHairFlow(Movement);
    }

    private void FixedUpdate()
    {
        Movement.UpdateMove(Time.fixedDeltaTime);
    }

    private void LateUpdate()
    {
        CameraFollow.UpdateFollow(Time.smoothDeltaTime);
    }

}
