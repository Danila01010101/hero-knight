using System;
using System.Xml;
using UnityEngine;
using UnityEngine.UIElements;

namespace Model
{
    public class BringerOfDeath : Monster
    {
        #region StateMachine
        private WonderState _wonderState;
        private ChaiseState _chaiseState;
        #endregion

        private float _attakDuration = 1f;

        public Action AttackStarted;
        private enum AnimationStates { Idle = 0, Walking = 1 }

        public BringerOfDeath(Transform transform, float movementSpeed) : base (transform, movementSpeed)
        {
            _deadState = new DeadState();
            _stateMachine = new StateMachine();
        }

        public void Initialize()
        {
            _wonderState = new WonderState(_movement, _moveSpeed);
            _wonderState.WonderStateEntered += delegate { ChangeAnimationState((int)AnimationStates.Walking); };
            _chaiseState = new ChaiseState(_movement, _attackRange, _moveSpeed, _attakDuration, AttackStarted);
            _stateMachine.Initialize(_wonderState);
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