using System.Collections;
using System.Collections.Generic;
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
        Time.fixedDeltaTime = 0.0166667f;
    }

    private void Update()
    {
        _movement.UpdateInput();
        _animation.UpdateAnimation(_movement);
    }

    private void FixedUpdate()
    {
        _movement.Move(Time.deltaTime);
    }

}
