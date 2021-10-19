using UnityEngine;

public class RunSystem : ISystem
{
    public void OnCreate()
    {
        throw new System.NotImplementedException();
    }

    public void OnUpdate()
    {
        throw new System.NotImplementedException();
    }

    private void Run(StateComponent state, in InputComponent input, float deltaTime)
    {
        var mult = state.OnGround ? 1.0f : 0.6f;
        // 当X轴速度超过最大速度且速度方向与移动方向一致时减速到最大速度
        if (Mathf.Abs(state.Speed.x) > Constants.MaxRun && Mathf.Sign(state.Speed.x) == input.MoveX)
        {
            state.Speed.x = ECSUtility.Approach(state.Speed.x, Constants.MaxRun * input.MoveX, Constants.RunReduce * mult * deltaTime);
        }
        else
        {
            state.Speed.x = ECSUtility.Approach(state.Speed.x, Constants.MaxRun * input.MoveX, Constants.RunAccel * mult * deltaTime);
        }
    }
}
