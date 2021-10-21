using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbSystem : ISystem
{
    public void OnCreate()
    {
        throw new System.NotImplementedException();
    }

    public void OnUpdate()
    {
        ClimbUpdate(null, null, null, 0);
    }

    private void ClimbUpdate(StateComponent state, ClimbComponent climb, InputComponent input, float deltaTime)
    {
        ClimbBegin(state, climb, input);
        if (!state.AgainstWall) return;
        if (!climb.CanUpdate) return;
        if (input.Climb && climb.ClimbTimer < climb.ClimbTime)
        {
            var target = 0.0f;
            if (input.MoveY > 0)
            {
                target = climb.UpSpeed;
            }
            else if (input.MoveY < 0)
            {
                target = climb.DownSpeed;
            }
            state.Speed.y = ECSUtility.Approach(state.Speed.y, target, climb.ClimbAccel * deltaTime);
            climb.ClimbTimer = Mathf.Min(climb.ClimbTimer + deltaTime, climb.ClimbTime);
        }
        else
        {
            ClimbEnd(climb);
        }
    }

    private void ClimbBegin(StateComponent state, ClimbComponent climb, InputComponent input)
    {
        if (climb.IsCooldown) return;
        if (!state.AgainstWall) return;
        if (!input.Climb || !climb.CanClimb) return;
        if (climb.HeldDownTimer < climb.ToleranceTime)
        {
            climb.CanClimb = false;
            climb.IsClimbing = true;
            climb.CanUpdate = true;
            state.Speed.x = 0;
            state.Speed.y *= climb.GrabYMult;
            climb.BeginEvent?.Invoke();
        }
    }

    private void ClimbEnd(ClimbComponent climb)
    {
        climb.ClimbTimer = 0.0f;
        climb.CanUpdate = false;
        climb.IsCooldown = true;
        climb.EndEvent?.Invoke();
    }

    private void ClimbLand(StateComponent state, ClimbComponent climb)
    {

    }
}
