using UnityEngine;

public class MoveSystem : MonoBehaviour
{
    public void OnCreate(MoveComponent move)
    {
        move = new MoveComponent();
    }

    public void OnUpdate(StateComponent state, MoveComponent move, InputComponent input, float deltaTime)
    {
        RunUpdate(state, move, input, deltaTime);
        GravityUpdate(state, move, input, deltaTime);
    }

    private void RunUpdate(StateComponent state, MoveComponent run, InputComponent input, float deltaTime)
    {
        var mult = state.OnGround ? run.GroundMult : run.AirMult;
        // 当X轴速度超过最大速度且速度方向与移动方向一致时减速到最大速度
        if (Mathf.Abs(state.Speed.x) > run.MaxRun && Mathf.Sign(state.Speed.x) == input.MoveX)
        {
            state.Speed.x = ECSUtility.Approach(state.Speed.x, run.MaxRun * input.MoveX, run.RunReduce * mult * deltaTime);
        }
        else
        {
            state.Speed.x = ECSUtility.Approach(state.Speed.x, run.MaxRun * input.MoveX, run.RunAccel * mult * deltaTime);
        }
    }

    private void GravityUpdate(StateComponent state, MoveComponent move, InputComponent input, float deltaTime)
    {
        var maxFall = move.MaxFall;
        //if (state.AgainstWall && input.MoveX == (int)state.Facing)
        //{
        //    maxFall = Mathf.MoveTowards(maxFall, climb.SlipSpeed, climb.SlideAccel * deltaTime);
        //}
        //else
        //{
        //    maxFall = Mathf.MoveTowards(maxFall, move.MaxFall, move.FallAccel * deltaTime);
        //}
        if (!state.OnGround)
        {
            //if (dash.IsFreezing) return;
            float mult = Mathf.Abs(state.Speed.y) < move.HalfGravThreshold && (state.IsRising || state.IsFalling) ? 0.5f : 1.0f;
            state.Speed.y = Mathf.MoveTowards(state.Speed.y, maxFall, move.Gravity * mult * deltaTime);
            //if (Mathf.Abs(_speed.y) > MinOffset) Console.LogFormat("ApplyGravity after speed Y {0:F3} OnGround {1}", _speed.y, _onGround);
        }
    }

}
