using UnityEngine;

public class StateSystem : MonoBehaviour
{
    public BoxCollider2D BoxCollider;

    public void OnCreate(StateComponent state)
    {
        state = new StateComponent();
    }

    public void OnUpdate(StateComponent state, InputComponent input)
    {
        FacingUpdate(state, input);
    }

    private void FacingUpdate(StateComponent state, InputComponent input)
    {
        if (input.MoveX != 0) state.Facing = (Facings)input.MoveX;
    }
}
