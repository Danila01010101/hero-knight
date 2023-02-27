using System;
using UnityEngine;

public class MonsterView : MonoBehaviour, IDamagable
{
    [SerializeField] private protected Rigidbody2D _rigidbody;
    [SerializeField] private protected Animator _animator;
    [SerializeField] private ParticleSystem _damageParticles;

    private Transform _transform;
    private const string AnimationStateName = "state";

    public Action<int> DamageTaken;

    protected virtual void Start()
    {
        _transform = transform;
    }

    public void SetAnimationState(int newState)
    {
        _animator.SetInteger(AnimationStateName, newState);
    }

    public void TakeDamage(int damage)
    {
        DamageTaken?.Invoke(damage);
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

    public void Move(Vector2 direction)
    {
        direction.y += _rigidbody.velocity.y;
        _rigidbody.velocity = direction;
    }

    public virtual void Hurt()
    {
        _animator.SetTrigger("Damaged");
    }

    public virtual void Die()
    {
        _animator.SetTrigger("Death");
    }

    public ParticleSystem GetDamageParticles()
    {
        return _damageParticles;
    }
}