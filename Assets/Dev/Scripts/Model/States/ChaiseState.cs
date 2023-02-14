using System;
using UnityEngine;

namespace Model
{
    public class ChaiseState : State
    {
        private Transform _targetTransform;

        private Movement _movement;

        private Action _attackAction;

        private bool _isAttacking = false;

        private float _attackRange;
        private float _chaiseSpeed;
        private float _attackCoolDown;
        private float _lastAttackTime;

        public ChaiseState(Movement movement, float attackRange, float chaiseSpeed, float attackCoolDown, Action attackAction) 
        {
            _movement = movement;
            _attackRange = attackRange;
            _chaiseSpeed = chaiseSpeed;
            _attackCoolDown = attackCoolDown;
            _lastAttackTime = -_attackCoolDown;
            _attackAction = attackAction;
        }

        public void DetectTarget(Transform  targetTransform)
        {
            _targetTransform = targetTransform;
        }

        public override void Enter() { }

        public override void Exit() { }

        public override void Update()
        {
            if (!_isAttacking && Time.time - _lastAttackTime >= _attackCoolDown)
            {
                Vector3 direction = Vector3.Normalize(_targetTransform.position - _movement.Transform.position);
                if (Vector2.Distance(_targetTransform.position, _movement.Transform.position) >= _attackRange)
                {
                    _movement.Move(_chaiseSpeed * direction);
                }
                else
                {
                    _movement.Move(direction);
                    _attackAction?.Invoke();
                    _isAttacking = true;
                    _lastAttackTime = Time.time;
                }
            }
        }

        public void EndAttack()
        {
            _isAttacking = false;
        }
    }
}