using UnityEngine;

public class InputSystem : ISystem
{
    public void OnCreate()
    {
        
    }

    public void OnUpdate()
    {
        UpdateKey();
    }

    private void UpdateKey()
    {
        InputEntity.Input.MoveX = Input.GetAxisRaw("Horizontal");
        InputEntity.Input.MoveY = Input.GetAxisRaw("Vertical");
        InputEntity.Input.Jump = Input.GetKey(KeyCode.C);
        InputEntity.Input.Dash = Input.GetKey(KeyCode.X);
        InputEntity.Input.Climb = Input.GetKey(KeyCode.Z);
    }
}
