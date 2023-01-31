using UnityEngine;



public class VirtualButton : MonoBehaviour
{

    private enum state
    {
        None,
        /// <summary>
        /// Pressed
        /// </summary>
        Enter,
        /// <summary>
        /// Check
        /// </summary>
        Stay,
        /// <summary>
        /// Released
        /// </summary>
        Exit,
    }

    [SerializeField]
    private bool _previous;
    private state _state;

    private float _bufferTime;
    private float _bufferCounter;

    public bool Value
    {
        private get { return _previous; }
        set
        {
            // 进入条件 true : false
            if (_previous && !value)         // release
            {
                _state = state.Exit;
            }
            // 进入条件 false : true
            if (!_previous && value)        // pressed
            {
                _bufferCounter = _bufferTime;
                _state = state.Enter;
            }
            // 进入条件 true : true
            else if (_previous && value)    // check
            {
                _bufferCounter -= Time.deltaTime;
                _state = state.Stay;
            }
            // 进入条件 false : false
            else
            {
                _bufferCounter = 0;
                _state = state.None;
            }
            _previous = value;
        }
    }

    /// <summary>
    /// 持续按下
    /// </summary>
    public bool Check => _state == state.Stay;

    /// <summary>
    /// 按钮按下
    /// </summary>
    public bool Pressed => _bufferCounter > 0 || _state == state.Enter;

    /// <summary>
    /// 按钮抬起
    /// </summary>
    public bool Released => _state == state.Exit;

    public VirtualButton(float bufferTime)
    {
        _bufferTime = bufferTime;
    }

    /// <summary>
    /// 按钮按下后清空buffer
    /// </summary>
    public void ConsumeBuffer()
    {
        _bufferCounter = 0.0f;
    }
}
