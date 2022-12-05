using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransComponent : IComponent
{
    public LayerMask GroundMask;    // 单项平台的检测
    public LayerMask PlatformMask;   // 双向平台的检测
    public Rigidbody2D Rigidbody;
    public BoxCollider2D BoxCollider;
}
