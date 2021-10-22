public class StateEntity : IEntity
{
    public int ID => throw new System.NotImplementedException();

    public InputComponent Input;
    public StateComponent State;
    public RaycastComponent Raycast;
}
