using System;
using System.Xml;
using UnityEngine;
using UnityEngine.UIElements;

namespace Model
{
    public class BringerOfDeath : Monster
    {
        #region StateMachine
        private StateMachine _stateMachine;
        private WonderState _wonderState;
        private ChaiseState _chaiseState;
        private DeadState _deadState;
        #endregion

        private Health _health;

        private float _attakDuration = 1f;

        public State CurrentState;
        public Action AttackStarted;
        public Action<int> AnimationStateChanged;
        public Health Health { get { return _health; } }
        public Movement Movement { get { return _movement; } }
        public enum AnimationStates { Idle = 0, Walking = 1 }

        private void Die()
        {
            _stateMachine.ChangeState(_deadState);
        }

        public BringerOfDeath(Transform transform, float movementSpeed)
        {
            _movement = new Movement(transform, movementSpeed);
            _health = new Health();
            _deadState = new DeadState();
            _stateMachine = new StateMachine();
            _health.Dying += Die;
        }

        public void Initialize()
        {
            _wonderState = new WonderState(_movement, _movement.Transform.position, _moveSpeed);
            _wonderState.WonderStateEntered += delegate { ChangeAnimationState(AnimationStates.Walking); };
            _chaiseState = new ChaiseState(_movement, _attackRange, _moveSpeed, _attakDuration, AttackStarted);
            _stateMachine.Initialize(_wonderState);
        }

        public void Update()
        {
            _stateMachine.Update();
        }

        public void ChangeAnimationState(AnimationStates state)
        {
            AnimationStateChanged?.Invoke((int)state);
        }

        public void DetectTargetToAttack(Transform enemyTransform)
        {
            if (!_health.IsAlive) return;

            _chaiseState.DetectTarget(enemyTransform);
            _stateMachine.ChangeState(_chaiseState);
        }

        public void LoseTarget()
        {
            if (!_health.IsAlive) return;

            _stateMachine.ChangeState(_wonderState);
        }

        public void EndAttack()
        {
            _chaiseState.EndAttack();
        }
    }
}