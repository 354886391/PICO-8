using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private InputController _input;
    [SerializeField] private CharacterMovement _movement;

    private void Update()
    {
        _movement.UpdateInput();
    }

    private void FixedUpdate()
    {
        _movement.Move(Time.fixedDeltaTime);
    }

}
