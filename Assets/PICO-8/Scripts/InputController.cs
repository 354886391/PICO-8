using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{

    public static bool Jump;
    public static float MoveX;
    public static float MoveY;

    public void UpdateInput()
    {
        Jump = Input.GetKey(KeyCode.Space);
        MoveX = Input.GetAxisRaw("Horizontal");
        MoveY = Input.GetAxisRaw("Vertical");
    }

}

