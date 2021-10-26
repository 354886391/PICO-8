using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{

    public InputComponent inputComponent;
    public RaycastComponent raycastComponent;
    public StateComponent stateComponent;
    public MoveComponent moveComponent;
    public JumpComponent jumpComponent;
    public DashComponent dashComponent;
    public ClimbComponent climbComponent;


    [SerializeField] private InputSystem input;
    [SerializeField] private RaycastSystem raycast;
    [SerializeField] private StateSystem state;
    [SerializeField] private MoveSystem move;
    [SerializeField] private JumpSystem jump;
    [SerializeField] private DashSystem dash;
    [SerializeField] private ClimbSystem climb;

    private void Start()
    {
        input.OnCreate(ref inputComponent);
        raycast.OnCreate(ref raycastComponent);
        state.OnCreate(ref stateComponent);
        move.OnCreate(ref moveComponent);
        jump.OnCreate(ref jumpComponent);
        dash.OnCreate(ref dashComponent);
        climb.OnCreate(ref climbComponent);
    }

    private void Update()
    {
        input.OnUpdate(inputComponent);
        state.OnUpdate(stateComponent, raycastComponent, inputComponent);
        raycast.OnUpdate(stateComponent, raycastComponent, Time.deltaTime);
        move.OnUpdate(stateComponent, moveComponent, inputComponent, Time.deltaTime);
        jump.OnUpdate(stateComponent, jumpComponent, inputComponent, Time.deltaTime);
        dash.OnUpdate(stateComponent, dashComponent, inputComponent, Time.deltaTime);
        climb.OnUpdate(stateComponent, climbComponent, inputComponent, Time.deltaTime);
    }

}
