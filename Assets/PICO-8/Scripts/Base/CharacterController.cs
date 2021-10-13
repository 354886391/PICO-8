#define NO_ENABLE_DEBUG
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private CharacterHealth _health;
    [SerializeField]
    private CharacterMovement _movement;
    [SerializeField]
    private CharacterAnimation _animation;
    [SerializeField]
    private CharacterHairFlow _hairFlow;
    [SerializeField]
    private CharacterCameraFollow _cameraFollow;


    private void Awake()
    {
        Application.targetFrameRate = 60;
        Time.fixedDeltaTime = 0.02f;
    }

    private void Start()
    {
        _health.Initialize(OnDeathbed);
    }

    private void Restart()
    {
        _hairFlow.AutoHideHairFlow(0.5f);
        _movement.AutoSetCanMove(0.5f, null);
        
        _movement.SetOriginPosition();
        _cameraFollow.SetOriginPosition();
        _animation.AnimateBorn();
        _health.Restart();
    }

    private void OnDeathbed()
    {
        Restart();
    }

    private void Update()
    {
        _movement.UpdateInput();
        _animation.UpdateAnimation(_movement);
        _hairFlow.UpdateHairFlow(_movement);
    }

    private void FixedUpdate()
    {
        _movement.UpdateMove(Time.fixedDeltaTime);
    }

    private void LateUpdate()
    {
        _cameraFollow.UpdateFollow(Time.smoothDeltaTime);
    }

}
