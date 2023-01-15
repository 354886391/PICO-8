using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualButton : MonoBehaviour
{

    [SerializeField]
    private bool _pressed;
    private float _pressedCounter;
    public System.Action<PressState> OnPressed;

    private bool canRepeat;
    private bool consumed;
    private float _bufferCounter;
    private float _repeatCounter;
    
    public bool Pressed
    {
        get { return _pressed; }
        private set
        {
            if (_pressed && !value)
            {
                _pressedCounter = 0;
                OnPressed?.Invoke(PressState.Exit);
            }
            else if (!_pressed && value)
            {
                _pressedCounter = 0;
                OnPressed?.Invoke(PressState.Enter);
            }
            _pressed = value;
            if (_pressed)
            {
                _pressedCounter += Time.deltaTime;
                OnPressed?.Invoke(PressState.Stay);
            }
        }
    }

    public float jumpPressTimer
    {
        get { return _pressedCounter; }
    }

    public VirtualButton(KeyCode key, float bufferTime)
    {

    }

}
