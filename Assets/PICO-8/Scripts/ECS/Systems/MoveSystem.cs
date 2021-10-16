using UnityEngine;

public class MoveSystem : ISystem
{
    public void OnCreate()
    {
        throw new System.NotImplementedException();
    }

    public void OnUpdate()
    {
        throw new System.NotImplementedException();
    }

    private void Move(MoveEntity moveEntity, InputEntity inputEntity, float deltaTime)
    {
        var move = moveEntity.Move;
        var input = inputEntity.Input;
        var speed = move.MoveSpeed;
        var maxRun = Constants.MaxRun;
        var runAccel = Constants.RunAccel;
        var runReduce = Constants.RunReduce;
        var mult = move.OnGround ? 1.0f : 0.6f;
        // 当X轴速度超过最大速度且速度方向与移动方向一致时减速
        if (Mathf.Abs(speed.x) > maxRun && Mathf.Sign(speed.x) == input.MoveX)
        {
            speed.x = ECSUtility.Approach(speed.x, maxRun * input.MoveX, runReduce * mult * deltaTime);
        }
        else
        {
            speed.x = ECSUtility.Approach(speed.x, maxRun * input.MoveX, runAccel * mult * deltaTime);
        }
    }
}
