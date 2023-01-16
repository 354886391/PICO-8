using UnityEngine;

public enum ButtonState
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

public class VirtualButton : MonoBehaviour
{

    [SerializeField]
    private bool _previous;
    private ButtonState _state;

    private float _bufferTime;
    private float _bufferCounter;

    /// <summary>
    /// True / False
    /// </summary>
    public bool Value
    {
        get { return _previous; }
        private set
        {
            // �������� true : false
            if (_previous && !value)         // release
            {
                _state = ButtonState.Exit;
            }
            // �������� false : true
            if (!_previous && value)        // pressed
            {
                _bufferCounter = _bufferTime;
                _state = ButtonState.Enter;
            }
            // �������� true : true
            else if (_previous && value)    // check
            {
                _bufferCounter -= Time.deltaTime;
                _state = ButtonState.Stay;
            }
            // �������� false : false
            else
            {
                _bufferCounter = 0;
                _state = ButtonState.None;
            }
            _previous = value;            
        }
    }

    public bool Check
    {
        get
        {
            return _state == ButtonState.Stay;
        }
    }

    public bool Pressed
    {
        get
        {
            return _bufferCounter > 0 || _state == ButtonState.Enter;
        }
    }

    public bool Released
    {
        get
        {
            return _state == ButtonState.Exit;
        }
    }

    public VirtualButton(KeyCode keyCode, float bufferTime)
    {
        _keyCode = keyCode;
        _bufferTime = bufferTime;
    }

}
