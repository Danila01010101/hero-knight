using UnityEngine;
using static PlayerAnimator;

namespace Model
{
    public class WanderingMonster : Monster
    {
        private WonderState _wonderState;
        private enum AnimationStates { Walking = 1, Dead = 2 }

        public WanderingMonster(Transform transform, float movementSpeed) : base(transform, movementSpeed) { }

        public void Initialize(float moveSpeed)
        {
            _wonderState = new WonderState(_movement, moveSpeed);
            _wonderState.WonderStateEntered += delegate { ChangeAnimationState((int)AnimationStates.Walking); };
            _stateMachine.Initialize(_wonderState);
        }
    }
}