public class InputComponent : Singleton<InputComponent>, IComponent
{

    public bool Jump { get; set; }
    public bool Dash { get; set; }
    public bool Climb { get; set; }
    public float MoveX { get; set; }
    public float MoveY { get; set; }

}
