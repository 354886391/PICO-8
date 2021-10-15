using UnityEngine;

public class InputSystem : ISystem
{
    public void OnCreate()
    {

    }

    public void OnUpdate()
    {

    }

    private void UpdateKey(InputEntity inputEntity)
    {
        var input = inputEntity.Input;

        input.MoveX = Input.GetAxisRaw("Horizontal");
        input.MoveY = Input.GetAxisRaw("Vertical");
        input.Jump = Input.GetKey(KeyCode.C);
        input.Dash = Input.GetKey(KeyCode.X);
        input.Climb = Input.GetKey(KeyCode.Z);
    }
}
