#define ENABLE_DEBUG
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    public void OnCreate(InputComponent input)
    {
        input = new InputComponent();
    }

    public void OnUpdate(InputComponent input, float deltaTime)
    {
        InputUpdate(input, deltaTime);
    }

    private void InputUpdate(InputComponent input, float deltaTime)
    {
        input.DeltaTime = deltaTime;
        input.MoveX = Input.GetAxisRaw("Horizontal");
        input.MoveY = Input.GetAxisRaw("Vertical");
        input.Jump = Input.GetKey(KeyCode.C);
        input.Dash = Input.GetKey(KeyCode.X);
        input.Climb = Input.GetKey(KeyCode.Z);
        UnitTest(input);
    }

    private void UnitTest(InputComponent input)
    {
        Console.LogFormat("InputComponent {0} {1} {2} {3} {4}", input.MoveX, input.MoveY, input.Jump, input.Dash, input.Climb);
    }
}
