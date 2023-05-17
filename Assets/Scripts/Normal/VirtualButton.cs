using UnityEngine;


public class VirtualButton : MonoBehaviour
{

    private enum StButton
    {
        None,
        Pressed,
        Check,
        Released,
    }

    [SerializeField]
    private bool _lastValue;    
    private float _bufferTime;      // 缓存时间
    private float _bufferCounter;   // 缓存计时

    private StButton _state;

    public bool Value
    {
        private get { return _lastValue; }
        set
        {
            // 进入条件 true : false
            if (_lastValue && !value)        // release(松开瞬间)
            {
                _state = StButton.Released;
            }
            // 进入条件 true : true
            else if (_lastValue && value)    // check
            {
                _bufferCounter -= Time.deltaTime;
                _state = StButton.Check;
            }
            // 进入条件 false : true
            else if (!_lastValue && value)   // pressed(按下瞬间)
            {
                _bufferCounter = _bufferTime;
                _state = StButton.Pressed;
            }
            // 进入条件 false : false
            else
            {
                _bufferCounter = 0;
                _state = StButton.None;
            }
            _lastValue = value;
        }
    }

    /// <summary>
    /// 持续按下
    /// </summary>
    public bool Check => _state == StButton.Check;

    /// <summary>
    /// 按钮按下
    /// </summary>
    public bool Pressed => _bufferCounter > 0 || _state == StButton.Pressed;

    /// <summary>
    /// 按钮抬起
    /// </summary>
    public bool Released => _state == StButton.Released;

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
