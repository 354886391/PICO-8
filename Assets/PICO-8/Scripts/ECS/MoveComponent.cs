using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Facings
{
    Left = -1,
    Right = 1,
}

public class MoveComponent
{
    public bool CanMove;
    public bool Freezing;
    public bool OnGround;
    public bool WasOnGround;
    public bool AgainstWall;   

    public Facings Facing;
    public Vector2 MoveSpeed;
}
