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

    private void Run(ref RunComponent run, in InputComponent input, float deltaTime)
    {
        var maxRun = Constants.MaxRun;
        var runAccel = Constants.RunAccel;
        var runReduce = Constants.RunReduce;
        var mult = run.OnGround ? 1.0f : 0.6f;
        // 当X轴速度超过最大速度且速度方向与移动方向一致时减速到最大速度
        if (Mathf.Abs(run.Speed.x) > maxRun && Mathf.Sign(run.Speed.x) == input.MoveX)
        {
            run.Speed.x = ECSUtility.Approach(run.Speed.x, maxRun * input.MoveX, runReduce * mult * deltaTime);
        }
        else
        {
            run.Speed.x = ECSUtility.Approach(run.Speed.x, maxRun * input.MoveX, runAccel * mult * deltaTime);
        }
    }
}
