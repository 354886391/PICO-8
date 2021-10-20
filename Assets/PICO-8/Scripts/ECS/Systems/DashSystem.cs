using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashSystem : ISystem
{
    public void OnCreate()
    {
        throw new System.NotImplementedException();
    }

    public void OnUpdate()
    {
        DashUpdate(null, null, null, 0.0f);
    }

    private void DashUpdate(StateComponent state, DashComponent dash, InputComponent input, float deltaTime)
    {
        DashBegin(state, dash, input);
        if (!dash.CanUpdate) return;
        if (dash.DashTimer < dash.DashTime)
        {
            if (dash.DashTimer < 0.05f)
            {
                dash.IsFreezing = true;
                state.Speed = Vector2.zero;
            }
            else
            {
                dash.IsFreezing = false;
                state.Speed = dash.Speed * dash.Direction;
            }
            dash.DashTimer = Mathf.Min(dash.DashTimer + deltaTime, dash.DashTime);
            dash.UpdateEvent?.Invoke();
        }
        else
        {
            DashEnd(dash);
        }
    }

    private void DashBegin(StateComponent state, DashComponent dash, InputComponent input)
    {
        if (dash.IsCooldown) return;
        if (!input.Dash || !dash.CanDash) return;
        if (dash.HeldDownTimer < dash.ToleranceTime)
        {
            dash.CanDash = false;
            dash.IsDashing = true;
            dash.CanUpdate = true;
            dash.Direction = new Vector2((int)state.Facing, input.MoveY);
            dash.BeginEvent?.Invoke();
        }
    }

    private void DashEnd(DashComponent dash)
    {
        dash.DashTimer = 0.0f;
        dash.IsCooldown = true;
        dash.CanUpdate = false;
        dash.EndEvent?.Invoke();
    }

    private void DashLand(StateComponent state, DashComponent dash)
    {
        if (state.OnGround && !state.WasOnGround)
        {
            dash.LandEvent?.Invoke();
        }
    }
}
