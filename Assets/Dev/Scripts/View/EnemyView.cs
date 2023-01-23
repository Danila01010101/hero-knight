using UnityEngine;
using UnityEngine.EventSystems;

namespace View
{
    public class EnemyView : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Animator _animator;

        private Vector2 _moveDirection;
        private Transform _transform;
        private const string AnimationStateName = "state";

        private void Start()
        {
            _transform = transform;
        }

        private void FixedUpdate()
        {
            Vector3 directionToMove = _moveDirection;
            directionToMove.y = _rigidbody.velocity.y;
            _rigidbody.velocity = directionToMove;
        }

        public Vector2 SetMoveDirection(Vector2 direction)
        {
            _moveDirection = direction;
            return _transform.position;
        }

        public void Flip()
        {
            Vector3 newRotation = transform.eulerAngles;
            if (transform.rotation.y == 0)
            {
                newRotation.y = 180;
            }
            else
            {
                newRotation.y = 0;
            }
            transform.eulerAngles = newRotation;
        }

        public void SetAnimationState(int newState)
        {
            _animator.SetInteger(AnimationStateName, newState);
        }
    }
}