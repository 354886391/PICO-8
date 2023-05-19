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
    private float _bufferTime;      // ����ʱ��
    private float _bufferCounter;   // �����ʱ

    private StButton _state;

    /// <summary>
    /// ����ֵ
    /// </summary>
    public bool Value
    {
        private get { return _lastValue; }
        set
        {
            if (_lastValue && !value)        // release(�ɿ�˲��)   �������� true : false
            {
                _state = StButton.Release;
            }
            else if (!_lastValue && value)   // pressed(����˲��)   �������� false : true
            {
                _bufferCounter = _bufferTime;
                _state = StButton.Press;
            }
            else if (_lastValue && value)    // stay               �������� true : true
            {
                _bufferCounter -= Time.deltaTime;
                _state = StButton.Stay;
            }
            else                             // none               �������� false : false
            {
                _bufferCounter = 0;
                _state = StButton.None;
            }
            _lastValue = value;
        }
    }

    /// <summary>
    /// ��ť����˲��
    /// </summary>
    public bool Pressed => _bufferCounter > 0 || _state == StButton.Press;

    /// <summary>
    /// ��ť�ͷ�˲��
    /// </summary>
    public bool Released => _state == StButton.Release;

    /// <summary>
    /// ��ť����ѹ��
    /// </summary>
    public bool Check => _state == StButton.Press || _state == StButton.Stay;

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
