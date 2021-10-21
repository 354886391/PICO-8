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
        JumpUpdate(null, null, null, 0);
    }

    private void JumpUpdate(StateComponent state, JumpComponent jump, InputComponent input, float deltaTime)
    {
        JumpBegin(state, jump, input);
        MidAirJumpBegin(state, jump, input);
        if (!jump.CanUpdate) return;
        if (input.Jump && jump.JumpTimer < jump.JumpTime)
        {
            state.Speed.y = jump.Speed;
            jump.JumpTimer = Mathf.Min(jump.JumpTimer + deltaTime, jump.JumpTime);
            jump.EndEvent?.Invoke();
        }
        else
        {
            JumpEnd(jump);
        }

    }

    private void JumpBegin(StateComponent state, JumpComponent jump, InputComponent input)
    {
        if (!state.OnGround) return;
        if (!input.Jump || !jump.CanJump) return;
        if (jump.HeldDownTimer < jump.ToleranceTime)
        {
            jump.CanJump = false;
            jump.IsJumping = true;
            jump.CanUpdate = true;
            state.Speed = new Vector2(jump.HBoost * input.MoveX, jump.Speed);
            jump.BeginEvent?.Invoke();
        }
    }

    private void MidAirJumpBegin(StateComponent state, JumpComponent jump, InputComponent input)
    {
        if (state.OnGround) return;
        if (!input.Jump || !jump.CanJump) return;
        if (jump.MidAirJumps > 0)
            jump.MidAirJumps = 0;
        if (jump.MidAirJumps < jump.MaxMidAirJump)
        {
            jump.MidAirJumps++;
            jump.CanJump = false;
            jump.IsJumping = true;
            jump.CanUpdate = true;
            jump.MidAirBeginEvent?.Invoke();
        }
    }

    private void JumpEnd(JumpComponent jump)
    {
        jump.JumpTimer = 0.0f;
        jump.CanUpdate = false;
        jump.EndEvent?.Invoke();
    }


    private void JumpLand(StateComponent state, JumpComponent jump)
    {
        if (state.OnGround && !state.WasOnGround)
        {
            jump.LandEvent?.Invoke();
        }
    }
}
