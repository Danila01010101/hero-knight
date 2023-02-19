using System;
using UnityEngine;

namespace Model
{
    public class WonderState : State
    {
        private Movement _movement;
        private Vector2 _patrolPosition;
        private float _patrollRadius = 3f;
        private float _patrollSpeed;
        private bool _isWalkingRight = false;

        public Action WonderStateEntered;

        public WonderState(Movement movement, float moveSpeed)
        {
            _movement = movement;
            _patrolPosition = movement.Transform.position;
            _patrollSpeed = moveSpeed;
        }

        public override void Enter()
        {
            WonderStateEntered?.Invoke();
        }

        public override void Exit() { }

        public override void Update()
        {
            if (_isWalkingRight)
            {
                if (_movement.Transform.position.x < _patrolPosition.x + _patrollRadius)
                {
                    _movement.Move(new Vector2(_patrollSpeed, 0));
                }
                else
                {
                    _isWalkingRight = false;
                }
            }
            else
            {
                if (_movement.Transform.position.x > _patrolPosition.x - _patrollRadius)
                {
                    _movement.Move(new Vector2(-_patrollSpeed, 0));
                }
                else
                {
                    _isWalkingRight = true;
                }
            }
        }
    }
}