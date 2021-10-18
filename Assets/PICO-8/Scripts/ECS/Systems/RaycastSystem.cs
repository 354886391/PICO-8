using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastSystem : MonoBehaviour
{
    public RaycastHit2D DetectGround(StateComponent state, RaycastComponent grounded, float deltaTime)
    {
        state.OnGround = false;
        var raycastHit = new RaycastHit2D();
        var goingUp = state.Movement.y > Constants.MinOffset;
        var origion = goingUp ? grounded.RayOrigin.topLeft : grounded.RayOrigin.bottomLeft;
        var direction = goingUp ? Vector2.up : Vector2.down;
        var distance = Mathf.Abs(state.Movement.y) + Constants.SkinWidth * 2f;
        for (int i = 0; i < grounded.HRaysCount; i++)
        {
            var rayOrigin = new Vector2(origion.x + grounded.HRaysInterval * i, origion.y);
            if (goingUp)
                raycastHit = Physics2D.Raycast(rayOrigin, direction, distance, grounded.GroundMask);    // 向上只检测单向平台
            else
                raycastHit = Physics2D.Raycast(rayOrigin, direction, distance, grounded.GroundMask | grounded.PlatformMask);    // 向下检测单双向平台
            Console.DrawRay(rayOrigin, direction * distance, Color.blue);
            if (raycastHit) { state.OnGround = true; break; }
        }
        return raycastHit;
    }

    public RaycastHit2D DetectWall(StateComponent state, RaycastComponent wall, float deltaTime)
    {
        state.AgainstWall = false;
        var raycastHit = new RaycastHit2D();
        var goingRight = state.Movement.x > Constants.MinOffset;
        var origin = goingRight ? wall.RayOrigin.bottomRight : wall.RayOrigin.bottomLeft;
        var direction = goingRight ? Vector2.right : Vector2.left;
        var distance = Mathf.Abs(state.Movement.x) + Constants.SkinWidth * 2f;
        for (int i = 0; i < wall.VRaysCount; i++)
        {
            var rayOrigin = new Vector2(origin.x, origin.y + wall.VRaysInterval * i);
            raycastHit = Physics2D.Raycast(rayOrigin, direction, distance, wall.GroundMask);
            Console.DrawRay(rayOrigin, direction * distance, Color.red);
            if (raycastHit) { state.AgainstWall = true; break; }
        }
        return raycastHit;
    }
}
