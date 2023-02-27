using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class PlayerView : MonoBehaviour, IDamagable
{
    [Header("Ground Checking")]
    [SerializeField] private LayerMask _contactLayer;
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private Transform _groundCheckPosition;
    [SerializeField] private Transform _attackCenterPosition;
    [SerializeField] private float _minGroundNormalY = 0.65f;
    [SerializeField] private float _gravityModifier = 1f;
    [Header("Attack")]
    [SerializeField] private LayerMask _enemiesLayer;
    [Space(5)]
    [SerializeField] private ParticleSystem _damageParticles;

    private float _attackRadius = 1f;
    private PlayerAnimator _playerAnimator;
    private Rigidbody2D _rigidbody;
    private Vector2 _targetVelocity;
    private Vector2 _velocity;
    private Vector2 _groundNormal;
     private ContactFilter2D _contactFilter;
    private RaycastHit2D[] _hitBuffer = new RaycastHit2D[16];
    private List<RaycastHit2D> _hitBufferList = new List<RaycastHit2D>(16);
    private const float _minMoveDistance = 0.001f;
    private const float _shellRadius = 0.01f;

    public Action<int> DamageDetected;

    private bool _isGrounded = true;

    private void Awake()
    {
        _playerAnimator = new PlayerAnimator(GetComponent<Animator>());
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _contactFilter.useTriggers = false;
        _contactFilter.SetLayerMask(_contactLayer);
        _contactFilter.useLayerMask = false;
    }

    private void FixedUpdate()
    {
        _velocity += _gravityModifier * Physics2D.gravity * Time.deltaTime;
        _velocity.x = _targetVelocity.x;

        _isGrounded = false;

        Vector2 deltaPosition = _velocity * Time.deltaTime;
        Vector2 moveAlongGround = new Vector2(_groundNormal.y, -_groundNormal.x);
        Vector2 move = moveAlongGround * deltaPosition.x;

        Movement(move, false);

        move = Vector2.up * deltaPosition.y;

        Movement(move, true);
    }

    public void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        if (distance > _minMoveDistance)
        {
            int count = _rigidbody.Cast(move, _contactFilter, _hitBuffer, distance + _shellRadius);

            _hitBufferList.Clear();

            for (int i = 0; i< count; i++)
            {
                _hitBufferList.Add(_hitBuffer[i]);
            }

            for (int i = 0; i < _hitBufferList.Count; i++)
            {
                Vector2 currentNormal = _hitBufferList[i].normal;
                if (currentNormal.y > _minGroundNormalY)
                {
                    _isGrounded = true;
                    if (yMovement)
                    {
                        _groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }

                    _playerAnimator.SetAnimationBoolean(PlayerAnimator.AnimationBoolean.IsGrounded, _isGrounded);
                }

                float projection = Vector2.Dot(_velocity, currentNormal);
                if (projection < 0)
                {
                    _velocity -= projection * currentNormal;
                }

                float modifiedDistance = _hitBufferList[i].distance - _shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }

            _rigidbody.position += move.normalized * distance;
        }


        if (_targetVelocity != Vector2.zero)
        {
            _playerAnimator.ChangeState(PlayerAnimator.AnimationState.Run);
        }
        else
        {
            _playerAnimator.ChangeState(PlayerAnimator.AnimationState.Idle);
        }
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

    public void Attack()
    {
        _playerAnimator.PlayAnimation(PlayerAnimator.AnimationAction.Attack);
    }

    public void DealDamage()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(_attackCenterPosition.position, _attackRadius, _enemiesLayer);

        foreach (Collider2D collider in detectedObjects)
        {
            IDamagable enemy;

            if (collider.TryGetComponent(out enemy))
            {
                enemy.TakeDamage(3);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_groundCheckPosition.transform.position, 0.25f);

        Handles.color = Color.yellow;
        if (_attackCenterPosition != null)
        {
            Handles.DrawWireDisc(_attackCenterPosition.position, Vector3.forward * 90, _attackRadius);
        }
    }

    public void TakeDamage(int value)
    {
        DamageDetected?.Invoke(value);
    }

    public void TakeDamage()
    {
        _playerAnimator.PlayAnimation(PlayerAnimator.AnimationAction.Damage);
    }

    public void DetectInput(Vector2 velocity)
    {
        _targetVelocity.x = velocity.x;
    }

    public void Jump(float jumpForce)
    {
        if (_isGrounded)
        {
            _velocity.y = jumpForce;
        }
    }

    public void Push(Vector2 force)
    {
        _rigidbody.AddForce(force, ForceMode2D.Impulse);
    }

    public void Die()
    {
        _playerAnimator.PlayAnimation(PlayerAnimator.AnimationAction.Death);
        StartCoroutine(ColliderDisabling());
    }

    private IEnumerator ColliderDisabling()
    {
        while (_rigidbody.velocity.y != 0)
        {
            yield return null;
        }
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.isKinematic = true;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<PlayerView>().enabled = false;
    }

    public ParticleSystem GetDamageParticles()
    {
        return _damageParticles;
    }
}