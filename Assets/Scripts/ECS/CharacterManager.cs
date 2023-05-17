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
        input.OnCreate(inputComponent);
        state.OnCreate(stateComponent);
        raycast.OnCreate(raycastComponent);
        move.OnCreate(moveComponent);
        jump.OnCreate(jumpComponent);
        dash.OnCreate(dashComponent);
        climb.OnCreate(climbComponent);
    }

    private void Update()
    {
        input.OnUpdate(inputComponent, Time.deltaTime);
        state.OnUpdate(stateComponent, inputComponent);
        raycast.OnUpdate(stateComponent, raycastComponent, Time.deltaTime);
        move.OnUpdate(stateComponent, moveComponent, inputComponent, Time.deltaTime);
        jump.OnUpdate(stateComponent, jumpComponent, inputComponent, Time.deltaTime);
        dash.OnUpdate(stateComponent, dashComponent, inputComponent, Time.deltaTime);
        climb.OnUpdate(stateComponent, climbComponent, inputComponent, Time.deltaTime);
        //StButton.MoveToPosition(stateComponent, Time.deltaTime);
    }

}
