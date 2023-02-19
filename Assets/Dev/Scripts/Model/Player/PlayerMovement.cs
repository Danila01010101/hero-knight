using System;
using System.Collections;
using UnityEngine;

namespace Model
{
    public class PlayerMovement : Movement
    {
        private float _jumpForce;
        private bool _isGrounded = true;
        private bool _canMove = true;
        private float _stunStarted;
        private float _stunDuration;
        private IEnumerator _currentStunCounter;

        public Action CharacterFell;
        public Action<float> CharacterJumped;

        public PlayerMovement(Transform transform, float movementSpeed, float jumpForce) : base (transform, movementSpeed)
        {
            _jumpForce = jumpForce;
        }

        public override void Move(Vector2 newMoveDirection)
        {
            if (!_canMove) return;

            base.Move(newMoveDirection);

            if (newMoveDirection.y >= 0.5f && _isGrounded)
            {
                _isGrounded = false;
                CharacterJumped?.Invoke(_jumpForce);
            }
            if (newMoveDirection.y <= -0.85f)
                CharacterFell?.Invoke();
        }

        public void DisableMovement()
        {
            _canMove = false;
        }

        public void DisableMovement(int duration)
        {
            if (_stunStarted + _stunDuration > Time.time + duration) return;

            _stunDuration= duration;
            _stunStarted = Time.time;

            if (_currentStunCounter != null)
                _currentStunCounter = EndMovementDisabling();
        }

        private IEnumerator EndMovementDisabling()
        {
            while (Time.time <= _stunStarted + _stunDuration)
            {
                yield return new WaitForEndOfFrame();
            }
            EnableMovement();
        }

        private void EnableMovement()
        {
            _canMove = true;
        }

        public void DetectGround()
        {
            _isGrounded = true;
        }
    }
}