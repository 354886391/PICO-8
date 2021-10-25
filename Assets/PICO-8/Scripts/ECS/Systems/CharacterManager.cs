using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private InputSystem input;
    private RaycastSystem raycast;
    private StateSystem state;
    private RunSystem run;
    private JumpSystem jump;
    private DashSystem dash;
    private ClimbSystem climb;

    private void Start()
    {
        
    }

    private void Update()
    {
        input.OnUpdate();
        raycast.OnUpdate();
        state.OnUpdate();
        run.OnUpdate();
        jump.OnUpdate();
        dash.OnUpdate();
        climb.OnUpdate();
    }

}
