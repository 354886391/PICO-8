using UnityEngine;

public class RunSystem : ISystem
{
    public void OnCreate()
    {
        throw new System.NotImplementedException();
    }

    public void OnUpdate()
    {
        RunUpdate(null, null, null, 0);
    }

    private void RunUpdate(StateComponent state, RunComponent run, in InputComponent input, float deltaTime)
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
}
