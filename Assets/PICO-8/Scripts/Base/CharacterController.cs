using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private CharacterMovement _movement;
    [SerializeField]
    private CharacterAnimation _animation;
    [SerializeField]
    private CharacterHairFlow _hairFlow;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        Time.fixedDeltaTime = 0.02f;
    }

    private void Update()
    {
        _movement.UpdateInput();
        _animation.UpdateAnimation(_movement);
        _hairFlow.UpdateHairFlow(_movement);
    }

    private void FixedUpdate()
    {
        _movement.Move(Time.fixedDeltaTime);
    }

}
