using UnityEngine;


public class VirtualButton : MonoBehaviour
{

    private enum StButton
    {
        None,
        Press,
        Stay,
        Release,
    }

    private bool _lastValue;
    private float _bufferTime;      // 缓存时间
    private float _bufferCounter;   // 缓存计时

    private StButton _state;

    /// <summary>
    /// 按键值
    /// </summary>
    public bool Value
    {
        private get { return _lastValue; }
        set
        {
            if (_lastValue && !value)        // release(松开瞬间)   进入条件 true : false
            {
                _state = StButton.Release;
            }
            else if (!_lastValue && value)   // pressed(按下瞬间)   进入条件 false : true
            {
                _bufferCounter = _bufferTime;
                _state = StButton.Press;
            }
            else if (_lastValue && value)    // stay               进入条件 true : true
            {
                _bufferCounter -= Time.deltaTime;
                _state = StButton.Stay;
            }
            else                             // none               进入条件 false : false
            {
                _bufferCounter = 0;
                _state = StButton.None;
            }
            _lastValue = value;
        }
    }

    /// <summary>
    /// 按钮按下瞬间
    /// </summary>
    public bool Pressed => _bufferCounter > 0 || _state == StButton.Press;

    /// <summary>
    /// 按钮释放瞬间
    /// </summary>
    public bool Released => _state == StButton.Release;

    /// <summary>
    /// 按钮持续压下
    /// </summary>
    public bool Check => _state == StButton.Press || _state == StButton.Stay;

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
