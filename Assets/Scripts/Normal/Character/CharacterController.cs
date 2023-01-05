#define NO_ENABLE_DEBUG
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    [SerializeField]
    public CharacterInput _input;
    [SerializeField]
    public CharacterMovement _movement;
    [SerializeField]
    public CharacterCameraFollow CameraFollow;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Start()
    {

    }

    private void Update()
    {
        _movement.Move(_input, Time.deltaTime);    // 左右移动
    }

    private void LateUpdate()
    {

    }
}
