#define NO_ENABLE_DEBUG
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    [SerializeField]
    public CharacterHealth _health;
    [SerializeField]
    public CharacterInput _input;
    [SerializeField]
    public CharacterMovement _movement;
    //[SerializeField]
    //public CharacterAnimation Animation;
    [SerializeField]
    public StateMachine _machine;
    [SerializeField]
    public CharacterHairFlow HairFlow;
    [SerializeField]
    public CharacterCameraFollow CameraFollow;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        _machine = new StateMachine(10);
        _machine.SetCallbacks(CharacterState.StNormal, update: NormalUpdate, begin: NormalBegin);
    }

    private void FixedUpdate()
    {
        _movement.DetectCollision(Time.fixedDeltaTime); // 检测碰撞信息
    }

    private void Update()
    {
        _movement.RunUpdate(_input, Time.deltaTime);    // 移动
    }

    private void LateUpdate()
    {

    }

    private void NormalBegin()
    {

    }

    private int NormalUpdate()
    {
        return 0;
    }

}
