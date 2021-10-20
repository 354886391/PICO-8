using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpSystem : ISystem
{
    public void OnCreate()
    {
        throw new System.NotImplementedException();
    }

    public void OnUpdate()
    {
        throw new System.NotImplementedException();
    }

    private void JumpUpdate(StateComponent state, JumpComponent jump, InputComponent input, float deltaTime)
    {

    }

    private void JumpBegin(StateComponent state, JumpComponent jump, InputComponent input)
    {
        if (!state.OnGround) return;
        if (!input.Jump || !jump.CanJump) return;
        if (jump.HeldDownTimer < jump.TOLERANCETIME)
        {
            jump.CanJump = false;
            jump.IsJumping = true;
            jump.CanUpdate = true;
            state.Speed = new Vector2(jump.HBOOST * input.MoveX, jump.SPEED);
            jump.BeginEvent?.Invoke();
        }
    }

    private void JumpEnd(JumpComponent jump)
    {
        jump.JumpTimer = 0.0f;
        jump.CanJump = false;
        jump.EndEvent?.Invoke();
    }

    private void JumpLand(StateComponent state, JumpComponent dash)
    {

    }
}
