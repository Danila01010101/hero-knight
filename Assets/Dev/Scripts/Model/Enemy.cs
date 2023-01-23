using System;
using UnityEngine;

namespace Model
{
    public class Enemy
    {
        private StateMachine _stateMachine;
        private WonderState _wonderState;
        private Transform _transform;
        private float _moveSpeed = 2f;

        public Action<int> AnimationStateChanged;
        public Transform Transform { get { return _transform; } }
        public enum AnimationStates { Idle = 0, Walking = 1 }

        public Enemy(Vector2 position, float movementSpeed)
        {
            _transform = new Transform(position, movementSpeed);
            _wonderState = new WonderState(_transform, position, _moveSpeed);
            _wonderState.WonderStateEntered += delegate { ChangeAnimationState(AnimationStates.Walking); };
            _stateMachine = new StateMachine();
        }

        public void Initialize()
        {
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
    }
}