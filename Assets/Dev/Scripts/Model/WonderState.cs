using System;
using UnityEngine;

namespace Model
{
    public class WonderState : IState
    {
        private Transform _transform;
        private Vector2 _patrolPosition;
        private float _patrollRadius = 3f;
        private float _patrollSpeed;
        private bool _isWalkingRight = false;

        public Action WonderStateEntered;

        public WonderState(Transform transform, Vector2 startWonderPosition, float moveSpeed)
        {
            _patrolPosition = startWonderPosition;
            _transform = transform;
            _patrollSpeed = moveSpeed;
        }

        public void Enter()
        {
            WonderStateEntered?.Invoke();
        }

        public void Exit()
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            if (_isWalkingRight)
            {
                if (_transform.CurrentPosition.x < _patrolPosition.x + _patrollRadius)
                {
                    _transform.Move(new Vector2(_patrollSpeed, 0));
                }
                else
                {
                    _isWalkingRight = false;
                }
            }
            else
            {
                if (_transform.CurrentPosition.x > _patrolPosition.x - _patrollRadius)
                {
                    _transform.Move(new Vector2(-_patrollSpeed, 0));
                }
                else
                {
                    _isWalkingRight = true;
                }
            }
        }
    }
}