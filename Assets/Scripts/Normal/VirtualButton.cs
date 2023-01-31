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
            // �������� true : false
            if (_previous && !value)         // release
            {
                _state = state.Exit;
            }
            // �������� false : true
            if (!_previous && value)        // pressed
            {
                _bufferCounter = _bufferTime;
                _state = state.Enter;
            }
            // �������� true : true
            else if (_previous && value)    // check
            {
                _bufferCounter -= Time.deltaTime;
                _state = state.Stay;
            }
            // �������� false : false
            else
            {
                _bufferCounter = 0;
                _state = state.None;
            }
            _previous = value;
        }
    }

    /// <summary>
    /// ��������
    /// </summary>
    public bool Check => _state == state.Stay;

    /// <summary>
    /// ��ť����
    /// </summary>
    public bool Pressed => _bufferCounter > 0 || _state == state.Enter;

    /// <summary>
    /// ��ţ̌��
    /// </summary>
    public bool Released => _state == state.Exit;

    public VirtualButton(float bufferTime)
    {
        _bufferTime = bufferTime;
    }

    /// <summary>
    /// ��ť���º����buffer
    /// </summary>
    public void ConsumeBuffer()
    {
        _bufferCounter = 0.0f;
    }
}
