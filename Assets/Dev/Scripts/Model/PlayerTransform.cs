using System;
using UnityEngine;

namespace Model
{
    public class PlayerTransform : Transform
    {
        private float _jumpForce;
        private bool _isGrounded = true;

        public Action CharacterFell;
        public Action<float> CharacterJumped;

        public PlayerTransform(Vector2 currentPosition, float movementSpeed, float jumpForce) : base (currentPosition, movementSpeed)
        {
            _jumpForce = jumpForce;
        }

        public override void Move(Vector2 newMoveDirection)
        {
            base.Move(newMoveDirection);

            if (newMoveDirection.y >= 0.85f && _isGrounded)
            {
                _isGrounded = false;
                CharacterJumped?.Invoke(_jumpForce);
            }
            if (newMoveDirection.y <= -0.85f)
                CharacterFell?.Invoke();
        }

        public void DetectGround()
        {
            _isGrounded = true;
        }
    }
}