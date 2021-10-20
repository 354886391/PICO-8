using UnityEngine;

public class RaycastSystem : ISystem
{


    public void OnCreate()
    {
        throw new System.NotImplementedException();
    }

    public void OnUpdate()
    {
        throw new System.NotImplementedException();
    }

    public void DetectGround(StateComponent state, RaycastComponent ground, float deltaTime)
    {
        state.OnGround = false;
        var goingUp = state.Movement.y > Constants.MinOffset;
        var rayOrigin = goingUp ? ground.RayOrigin.topLeft : ground.RayOrigin.bottomLeft;
        var direction = goingUp ? Vector2.up : Vector2.down;
        var distance = Mathf.Abs(state.Movement.y) + Constants.SkinWidth * 2f;
        for (int i = 0; i < ground.HRaysCount; i++)
        {
            var origin = new Vector2(rayOrigin.x + ground.HRaysInterval * i, rayOrigin.y);
            var layMask = goingUp ? ground.GroundMask : (LayerMask)(ground.GroundMask | ground.PlatformMask);
            ground.RaycastGround = Physics2D.Raycast(origin, direction, distance, layMask);    // 向上检测单向平台 向下检测单双向平台
            Console.DrawRay(origin, direction * distance, Color.red);
            if (ground.RaycastGround) { state.OnGround = true; break; }
        }
    }

    public void DetectWall(StateComponent state, RaycastComponent wall, float deltaTime)
    {
        state.AgainstWall = false;
        var goingRight = state.Movement.x > Constants.MinOffset;
        var rayOrigin = goingRight ? wall.RayOrigin.bottomRight : wall.RayOrigin.bottomLeft;
        var direction = goingRight ? Vector2.right : Vector2.left;
        var distance = Mathf.Abs(state.Movement.x) + Constants.SkinWidth * 2f;
        for (int i = 0; i < wall.VRaysCount; i++)
        {
            var origin = new Vector2(rayOrigin.x, rayOrigin.y + wall.VRaysInterval * i);
            wall.RaycastWall = Physics2D.Raycast(origin, direction, distance, wall.GroundMask);
            Console.DrawRay(origin, direction * distance, Color.yellow);
            if (wall.RaycastWall) { state.AgainstWall = true; break; }
        }
    }
}
