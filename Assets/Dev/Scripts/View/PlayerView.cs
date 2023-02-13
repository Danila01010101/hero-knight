using System;
using UnityEditor;
using UnityEngine;
using View;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerView : MonoBehaviour
{
    [Header("Ground Checking")]
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private Transform _groundCheckPosition;
    [SerializeField] private Transform _attackCenterPosition;
    [SerializeField] private float _groundCheckRadius = 0.25f;

    private float _attackRadius = 1f;
    private bool _isCheckingGround = true;
    private PlayerAnimator _playerAnimator;
    private Rigidbody2D _rigidbody;
    private Vector2 _moveDirection;

    public Action GroundDetected;
    public Action<int> DamageDetected;

    private void Awake()
    {
        _playerAnimator = new PlayerAnimator(GetComponent<Animator>());
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector3 directionToMove = _moveDirection;
        directionToMove.y = _rigidbody.velocity.y;
        _rigidbody.velocity = directionToMove;

        if (_isCheckingGround && Physics2D.OverlapCircle(_groundCheckPosition.position, _groundCheckRadius, _groundLayerMask) != null)
        {
            _isCheckingGround = false;
            _playerAnimator.SetAnimationBoolean(PlayerAnimator.AnimationBoolean.IsGrounded, true);
            GroundDetected?.Invoke();
        }
    }

    public void SetMoveDirection(Vector2 newDirection)
    {
        _moveDirection = newDirection;
        if (_moveDirection != Vector2.zero)
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

    public void Jump(float jumpForce)
    {
        _isCheckingGround = true;
        Vector3 newVelocity = _rigidbody.velocity;
        newVelocity.y = jumpForce;
        _rigidbody.velocity = newVelocity;
        _playerAnimator.SetAnimationBoolean(PlayerAnimator.AnimationBoolean.IsGrounded, false);
        _playerAnimator.PlayAnimation(PlayerAnimator.AnimationAction.Jump);
    }

    public void Attack()
    {
        _playerAnimator.PlayAnimation(PlayerAnimator.AnimationAction.Attack);
    }

    public void DealDamage()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(_attackCenterPosition.position, _attackRadius);

        foreach (Collider2D collider in detectedObjects)
        {
            EnemyView enemy;

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

    public void Die()
    {
        _playerAnimator.PlayAnimation(PlayerAnimator.AnimationAction.Death);
    }
}