using UnityEngine;

public class DashSystem : MonoBehaviour
{
    public void OnCreate(DashComponent dash)
    {
        dash = new DashComponent();
    }

    public void OnUpdate(StateComponent state, DashComponent dash, InputComponent input, float deltaTime)
    {
        DashUpdate(state, dash, input, deltaTime);
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
        if (!input.Dash || !input.CanDash) return;
        if (input.HeldDownTimer < dash.ToleranceTime)
        {
            input.CanDash = false;
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
        dash.IsDashing = false;
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
