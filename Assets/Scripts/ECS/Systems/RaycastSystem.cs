using UnityEngine;

public class RaycastSystem : MonoBehaviour
{
    public BoxCollider2D BoxCollider;

    public void OnCreate(RaycastComponent raycast)
    {
        raycast = new RaycastComponent();
        ComputeRayBounds(raycast);
        ComputeRaysInterval(raycast);
    }

    public void OnUpdate(StateComponent state, RaycastComponent raycast, float deltaTime)
    {
        ComputeRayBounds(raycast);
        DetectGround(state, raycast, deltaTime);
        DetectWall(state, raycast, deltaTime);
    }

    private void ComputeRayBounds(RaycastComponent raycast)
    {
        var bounds = BoxCollider.bounds;
        bounds.Expand(Constants.SkinWidth * -2f);
        raycast.Origin.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycast.Origin.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycast.Origin.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
    }

    private void ComputeRaysInterval(RaycastComponent raycast)
    {
        raycast.HRaysInterval = (raycast.Origin.bottomRight.x - raycast.Origin.bottomLeft.x) / (raycast.HRaysCount - 1);
        raycast.VRaysInterval = (raycast.Origin.topLeft.y - raycast.Origin.bottomLeft.y) / (raycast.VRaysCount - 1);
    }

    public void DetectGround(StateComponent state, RaycastComponent ground, float deltaTime)
    {
        state.OnGround = false;
        var movement = state.Speed * deltaTime;
        var goingUp = movement.y > Constants.MinOffset;
        var rayOrigin = goingUp ? ground.Origin.topLeft : ground.Origin.bottomLeft;
        var direction = goingUp ? Vector2.up : Vector2.down;
        var distance = Mathf.Abs(movement.y) + Constants.SkinWidth * 2f;
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
        var movement = state.Speed * deltaTime;
        var goingRight = movement.x > Constants.MinOffset;
        var rayOrigin = goingRight ? wall.Origin.bottomRight : wall.Origin.bottomLeft;
        var direction = goingRight ? Vector2.right : Vector2.left;
        var distance = Mathf.Abs(movement.x) + Constants.SkinWidth * 2f;
        for (int i = 0; i < wall.VRaysCount; i++)
        {
            var origin = new Vector2(rayOrigin.x, rayOrigin.y + wall.VRaysInterval * i);
            wall.RaycastWall = Physics2D.Raycast(origin, direction, distance, wall.GroundMask);
            Console.DrawRay(origin, direction * distance, Color.yellow);
            if (wall.RaycastWall) { state.AgainstWall = true; break; }
        }
    }
}
