using System;
using UnityEngine;

namespace Model
{
    public abstract class Monster
    {
        #region StateMachine
        private protected StateMachine _stateMachine;
        private protected DeadState _deadState;
        #endregion

        private protected float _moveSpeed = 2f;
        private protected float _attackRange = 2f;

        private protected Movement _movement;
        private protected Health _health;

        public Action<int> AnimationStateChanged;
        public Health Health { get { return _health; } }
        public Movement Movement { get { return _movement; } }

        public Monster(Transform transform, float movementSpeed)
        {
            _movement = new Movement(transform, movementSpeed);
            _deadState = new DeadState();
            _stateMachine = new StateMachine();
            _health = new Health();
            _health.Dying += Die;
        }

        private void Die()
        {
            _stateMachine.ChangeState(_deadState);
        }

        public void Update()
        {
            _stateMachine.Update();
        }

        public void ChangeAnimationState(int state)
        {
            AnimationStateChanged?.Invoke(state);
        }
    }
}