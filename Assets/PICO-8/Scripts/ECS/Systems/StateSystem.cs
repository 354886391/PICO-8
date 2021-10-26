using UnityEngine;

public class StateSystem : MonoBehaviour
{
    public void OnCreate(ref StateComponent state)
    {
        state = new StateComponent();
    }

    public void OnUpdate( StateComponent state,  RaycastComponent raycast,  InputComponent input)
    {
        StateUpate( state,  raycast,  input);
    }

    private void StateUpate( StateComponent state,  RaycastComponent raycast,  InputComponent input)
    {
        FacingUpdate( state,  input);
    }

    private void FacingUpdate( StateComponent state,  InputComponent input)
    {
        if (input.MoveX != 0) state.Facing = (Facings)input.MoveX;
    }
}
