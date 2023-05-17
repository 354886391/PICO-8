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
    private float _bufferTime;      // ����ʱ��
    private float _bufferCounter;   // �����ʱ

    private StButton _state;

    public bool Value
    {
        private get { return _lastValue; }
        set
        {
            // �������� true : false
            if (_lastValue && !value)        // release(�ɿ�˲��)
            {
                _state = StButton.Released;
            }
            // �������� true : true
            else if (_lastValue && value)    // check
            {
                _bufferCounter -= Time.deltaTime;
                _state = StButton.Check;
            }
            // �������� false : true
            else if (!_lastValue && value)   // pressed(����˲��)
            {
                _bufferCounter = _bufferTime;
                _state = StButton.Pressed;
            }
            // �������� false : false
            else
            {
                _bufferCounter = 0;
                _state = StButton.None;
            }
            _lastValue = value;
        }
    }

    /// <summary>
    /// ��������
    /// </summary>
    public bool Check => _state == StButton.Check;

    /// <summary>
    /// ��ť����
    /// </summary>
    public bool Pressed => _bufferCounter > 0 || _state == StButton.Pressed;

    /// <summary>
    /// ��ţ̌��
    /// </summary>
    public bool Released => _state == StButton.Released;

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
