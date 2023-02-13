using System;
using UnityEditor;
using UnityEngine;

namespace View
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class EnemyView : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _attackCenter;
        [SerializeField] private Transform _detectCenter;
        [SerializeField] private Vector2 _detectSize;
        [SerializeField] private float _attackRadius;

        private Transform _transform;
        private SpriteRenderer _spriteRenderer;
        private const string AnimationStateName = "state";

        public Action AttackEnded;
        public Action EnemyLost;
        public Action<int> DamageTaken;
        public Action<Transform> EnemyDetected;

        private void Start()
        {
            _transform = transform;
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Move(Vector2 direction)
        {
            direction.y += _rigidbody.velocity.y;
            _rigidbody.velocity = direction;
        }

        public void Flip()
        {
            Vector3 newRotation = _transform.eulerAngles;
            if (_transform.rotation.y == 0)
            {
                newRotation.y = 180;
            }
            else
            {
                newRotation.y = 0;
            }
            _transform.eulerAngles = newRotation;
        }

        public void SetAnimationState(int newState)
        {
            _animator.SetInteger(AnimationStateName, newState);
        }

        public void Attack()
        {
            _animator.SetTrigger("Attack");
        }

        public void DealDamage()
        {
            Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(_attackCenter.position, _attackRadius);

            foreach (Collider2D collider in detectedObjects)
            {
                PlayerView characterView;

                if (collider.TryGetComponent(out characterView))
                {
                    characterView.TakeDamage(3);
                }
            }
        }

        public void TakeDamage(int damage)
        {
            _animator.SetTrigger("Damaged");
            DamageTaken?.Invoke(damage);
        }

        public void Die()
        {
            _animator.SetTrigger("Death");
        }

        public void DetectAttackEnd()
        {
            AttackEnded?.Invoke();
            _animator.SetTrigger("AttackEnded");
        }

        public void HideRenderer()
        {
            _spriteRenderer.enabled = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            PlayerView player;

            if (collision.gameObject.TryGetComponent(out player))
            {
                EnemyDetected?.Invoke(player.transform);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            PlayerView player;

            if (collision.gameObject.TryGetComponent(out player))
            {
                EnemyLost?.Invoke();
            }
        }

        private void OnDrawGizmos()
        {
            Handles.color = Color.red;
            if (_detectCenter != null)
            {
                Handles.DrawWireCube(_detectCenter.position, _detectSize);
            }
            Handles.color = Color.yellow;
            if (_attackCenter != null)
            {
                Handles.DrawWireDisc(_attackCenter.position, Vector3.forward * 90, _attackRadius);
            }
        }
    }
}