using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{

    public float MoveX;
    public float MoveY;
    public bool Jump;


    private void HandleInput()
    {
        MoveX = Input.GetAxisRaw("Horizontal");
        MoveY = Input.GetAxisRaw("Vertical");
    }

    protected class JumpCheck
    {
        protected bool _canJump = true;
        protected bool _jump;
        protected bool _isJumping;
        protected bool _updateJumpTimer;
        protected float _jumpTimer;
        protected float _jumpHeldDownTimer;
        protected float _jumpToleranceTime = 0.15f;
        protected float _extraJumpTime = 0.5f;
        protected float _extraJumpPower = 25.0f;
        protected int _midAirJumpCount;
        protected int _maxMidAirJumps = 1;


        public bool Jump
        {
            get { return _jump; }
            set
            {
                // If jump is released, allow to jump again
                if (_jump && value == false)
                {
                    _canJump = true;
                    _jumpHeldDownTimer = 0.0f;
                }
                // Update jump value; if pressed, update held down timer
                _jump = value;
                if (_jump)
                {
                    _jumpHeldDownTimer += Time.deltaTime;
                }
            }
        }

        public bool IsJumping
        {
            get
            {
                if (_isJumping && speed.y < 0.0001f)
                {
                    _isJumping = false;
                }
                return _isJumping;
            }
        }

        public bool IsFalling
        {
            get { return !onGround && speed.y < 0.0001f; }
        }


        protected void Jumping()
        {
            if (!_jump || !_canJump) return;
            if (_jumpHeldDownTimer > _jumpToleranceTime) return;
            _canJump = false;
            _isJumping = true;
            _updateJumpTimer = true;
            // Apply jump impulse

        }

        protected void MidAirJumping()
        {
            if (_midAirJumpCount > 0 && onGround)
            {
                _midAirJumpCount = 0;
            }
            if (!_jump || !_canJump) return;
            if (_midAirJumpCount >= _maxMidAirJumps) return;
            _midAirJumpCount++;
            _canJump = false;
            _isJumping = true;
            _updateJumpTimer = true;
            // Apply jump impulse

        }

        protected void UpdateJumpTimer()
        {
            if (!_updateJumpTimer) return;
            if (_jump && _jumpTimer < _extraJumpTime)
            {
                var jumpProcess = _jumpTimer / _extraJumpTime;
                var proportionaljumpPower = Mathf.Lerp(_extraJumpPower, 0.0f, jumpProcess);
                //movement.(Vector3.up * proportionalJumpPower, ForceMode.Acceleration);
                _jumpTimer = Mathf.Min(_jumpTimer + Time.deltaTime, _extraJumpTime);
            }
            else
            {
                _jumpTimer = 0.0f;
                _updateJumpTimer = false;
            }
        }

        protected void Move()
        {
            //movement.Move();
            Jumping();
            MidAirJumping();
            UpdateJumpTimer();
        }

    }
}
