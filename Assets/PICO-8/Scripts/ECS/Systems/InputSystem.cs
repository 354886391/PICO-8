using UnityEngine;

public class InputSystem : MonoBehaviour
{
    public void OnCreate( InputComponent input)
    {
        input = new InputComponent();
    }

    public void OnUpdate( InputComponent input)
    {
        InputUpdate( input);
    }

    private void InputUpdate( InputComponent input)
    {
        input.MoveX = Input.GetAxisRaw("Horizontal");
        input.MoveY = Input.GetAxisRaw("Vertical");
        input.Jump = Input.GetKey(KeyCode.C);
        input.Dash = Input.GetKey(KeyCode.X);
        input.Climb = Input.GetKey(KeyCode.Z);
    }
}
