using System;
using UnityEngine;

namespace Model
{
    public class Player
    {
        private float _movementSpeed;
        private float _jumpForce;
        private bool _isGrounded = true;
        private Vector3 _direction;
        private Directions _currentFaceDirection = Directions.Right;
        private enum Directions { Right, Left }

        public Action OnDirectionChange;
        public Action CharacterFell;
        public Action<float> CharacterJumped;
        public Action<Vector3> CharacterMoved;

        public Player(float movementSpeed, float jumpForce)
        {
            _movementSpeed = movementSpeed;
            _jumpForce = jumpForce;
        }

        public void Move(Vector3 newMoveDirection)
        {
            Directions newFaceDirection = _currentFaceDirection;

            if (newMoveDirection.x > 0) 
                newFaceDirection = Directions.Right;
            if (newMoveDirection.x < 0) 
                newFaceDirection = Directions.Left;
            if (newFaceDirection != _currentFaceDirection)
            {
                OnDirectionChange?.Invoke();
                _currentFaceDirection = newFaceDirection;
            }
            _direction.x = newMoveDirection.x;
            CharacterMoved?.Invoke(_direction * _movementSpeed);

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