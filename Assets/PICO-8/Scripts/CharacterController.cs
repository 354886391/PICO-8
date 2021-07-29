using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private CharacterMovement _movement;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        _movement.UpdateInput();
    }

    private void FixedUpdate()
    {
        _movement.Move(Time.fixedDeltaTime);
    }

}
