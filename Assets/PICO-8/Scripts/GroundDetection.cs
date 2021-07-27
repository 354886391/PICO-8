using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public struct GroundHit
{
    public bool isGround;
    public Vector2 groundPoint;
    public Vector2 groundNormal;
    public float groundDistance;
    public Collider2D groundCollider;
    public Rigidbody2D groundRigidbody;

    public void ResetInfo()
    {
        
    }

    public void SetFrom(RaycastHit2D hitInfo)
    {
        groundPoint = hitInfo.point;
        groundNormal = hitInfo.normal;
        groundDistance = hitInfo.distance;
        groundCollider = hitInfo.collider;
        groundRigidbody = hitInfo.rigidbody;
    }
}

public class GroundDetection : MonoBehaviour
{

    public GroundHit groundHit;
    public LayerMask groundMask;
    //public bool isGround;
    //public float groundDistance;
    //public Vector3 groundPoint;
    //public Vector3 groundNormal;

    //public Collider2D groundCollider;
    //public Rigidbody2D groundRigidbody;

    [SerializeField] private int _rayCount = 5;
    [SerializeField] private float _rayInterval;
    [SerializeField] private float _skinWidth = 0.02f;
    [SerializeField] private float _minOffset = 0.001f;
    [SerializeField] private BoxCollider2D _boxCollider;

    public bool ComputeGroundHit(ref GroundHit groundHitInfo, float distance = Mathf.Infinity)
    {
        var origin = _boxCollider.bounds.min;
        for (int i = 0; i < _rayCount; i++)
        {
            var rayOrigin = new Vector2(origin.x + _rayInterval * i, origin.y - _skinWidth);
            var raycastHit = Physics2D.Raycast(rayOrigin, Vector2.down, distance, groundMask);
            Console.DrawRay(rayOrigin, Vector2.down * distance, Color.red);
            if (raycastHit && raycastHit.distance < 0.001f)
            {
                groundHitInfo.isGround = true;
                groundHitInfo.SetFrom(raycastHit);
                return true;
            }
        }
        return false;
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    protected void DrawGizmos()
    {
        if (!Application.isPlaying) return;
        if (!groundHit.isGround) return;
        // ground point
        Handles.color = new Color(0.0f, 1.0f, 0.0f, 0.25f);
        Handles.DrawSolidDisc(groundHit.groundPoint, groundHit.groundNormal, 0.1f);
        // ground normal
        Gizmos.color = groundHit.isGround ? Color.green : Color.blue;
        Gizmos.DrawRay(groundHit.groundPoint, groundHit.groundNormal);
    }

    private void OnDrawGizmosSelected()
    {
        DrawGizmos();
    }

    private void FixedUpdate()
    {
        //ComputeGroundHit();
    }
}
