public class CharacterEntity : IEntity
{
    public int ID => throw new System.NotImplementedException();

    public static InputComponent inputComponent;
    public static RaycastComponent raycastComponent;
    public static StateComponent stateComponent;
    public static MoveComponent moveComponent;
    public static JumpComponent jumpComponent;
    public static DashComponent dashComponent;
    public static ClimbComponent climbComponent;
}
