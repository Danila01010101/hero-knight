using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerView : MonoBehaviour
{
    [Header("Ground Checking")]
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private Transform _groundCheckPosition;
    [SerializeField] private float _groundCheckRadius = 0.25f;

    private bool _isCheckingGround = true;
    private PlayerAnimator _playerAnimator;
    private Rigidbody2D _rigidbody;
    private Vector2 _moveDirection;

    public Action GroundDetected;

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

    public Vector2 SetMoveDirection(Vector2 newDirection)
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
        return new Vector2(transform.position.x, transform.position.y);
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(_groundCheckPosition.transform.position, 0.25f);
    }
}