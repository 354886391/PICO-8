﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{

    public static InputComponent inputComponent;
    public static RaycastComponent raycastComponent;
    public static StateComponent stateComponent;
    public static MoveComponent moveComponent;
    public static JumpComponent jumpComponent;
    public static DashComponent dashComponent;
    public static ClimbComponent climbComponent;


    [SerializeField] private InputSystem input;
    [SerializeField] private RaycastSystem raycast;
    [SerializeField] private StateSystem state;
    [SerializeField] private MoveSystem move;
    [SerializeField] private JumpSystem jump;
    [SerializeField] private DashSystem dash;
    [SerializeField] private ClimbSystem climb;

    private void Start()
    {
        input.OnCreate( inputComponent);
        raycast.OnCreate( raycastComponent);
        state.OnCreate( stateComponent);
        move.OnCreate( moveComponent);
        jump.OnCreate( jumpComponent);
        dash.OnCreate( dashComponent);
        climb.OnCreate( climbComponent);
    }

    private void Update()
    {
        input.OnUpdate( inputComponent);
        state.OnUpdate( stateComponent,  raycastComponent,  inputComponent);
        raycast.OnUpdate( stateComponent,  raycastComponent, Time.deltaTime);
        move.OnUpdate( stateComponent,  moveComponent,  inputComponent, Time.deltaTime);
        jump.OnUpdate( stateComponent,  jumpComponent,  inputComponent, Time.deltaTime);
        dash.OnUpdate( stateComponent,  dashComponent,  inputComponent, Time.deltaTime);
        climb.OnUpdate( stateComponent,  climbComponent,  inputComponent, Time.deltaTime);
    }

}
