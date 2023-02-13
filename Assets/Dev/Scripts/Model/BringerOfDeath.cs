using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Model
{
    public class BringerOfDeath : Monster
    {
        private StateMachine _stateMachine;
        private WonderState _wonderState;
        private ChaiseState _chaiseState;

        private Health _health;

        private float _attakDuration = 1f;

        public State CurrentState;
        public Action AttackStarted;
        public Action<int> AnimationStateChanged;
        public Health Health { get { return _health; } }
        public Movement Movement { get { return _movement; } }
        public enum AnimationStates { Idle = 0, Walking = 1 }

        public BringerOfDeath(Transform transform, float movementSpeed)
        {
            _movement = new Movement(transform, movementSpeed);
            _health = new Health();
            _stateMachine = new StateMachine();
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
            _chaiseState.DetectTarget(enemyTransform);
            _stateMachine.ChangeState(_chaiseState);
        }

        public void LoseTarget()
        {
            _stateMachine.ChangeState(_wonderState);
        }

        public void EndAttack()
        {
            if (_chaiseState != null)
                _chaiseState.EndAttack();
        }
    }
}