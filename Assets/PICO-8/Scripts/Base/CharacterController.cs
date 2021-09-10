using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private CharacterMovement _movement;
    [SerializeField]
    private CharacterAnimation _animation;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        Time.fixedDeltaTime = 0.02f;
    }

    private void Update()
    {
        _movement.UpdateInput();
        _animation.UpdateAnimation(_movement);

    }

    private void FixedUpdate()
    {
        _movement.Move(Time.fixedDeltaTime);
    }

}
