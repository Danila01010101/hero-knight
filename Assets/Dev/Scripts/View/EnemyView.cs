using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace View
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class EnemyView : MonsterView
    {
        [SerializeField] private CapsuleCollider _collider;
        [SerializeField] private Transform _attackCenter;
        [SerializeField] private Transform _detectCenter;
        [SerializeField] private Vector2 _detectSize;
        [SerializeField] private float _attackRadius;

        private SpriteRenderer _spriteRenderer;

        public Action AttackEnded;
        public Action EnemyLost;
        public Action<Transform> EnemyDetected;

        protected override void Start()
        {
            base.Start();
            _spriteRenderer = GetComponent<SpriteRenderer>();
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

        public override void Die()
        {
            base.Die();
            StartCoroutine(ColliderDisabling());
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

        private IEnumerator ColliderDisabling()
        {
            while (_rigidbody.velocity.y != 0)
            {
                yield return null;
            }
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.isKinematic = true;
            GetComponent<EnemyView>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }
    }
}