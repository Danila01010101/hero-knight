using Model;
using UnityEngine;

public class WanderingMonsterController : MonoBehaviour
{
    [SerializeField] private WanderingMonsterView _enemyView;
    [SerializeField] private float _movementSpeed = 1f;

    private WanderingMonster _monster;

    private void Awake()
    {
        _monster = new WanderingMonster(transform, _movementSpeed);
    }

    private void Start()
    {
        _monster.Initialize(_movementSpeed);
    }

    private void Update()
    {
        _monster.Update();
    }

    private void OnEnable()
    {
        _monster.Movement.Moved += _enemyView.Move;
        _monster.Movement.OnDirectionChange += _enemyView.Flip;
        _monster.AnimationStateChanged += _enemyView.SetAnimationState;
        _enemyView.DamageTaken += _monster.Health.TakeDamage;
        _monster.Health.Hurt += _enemyView.Hurt;
        _monster.Health.Dying += _enemyView.Die;
    }

    private void OnDisable()
    {
        _monster.Movement.Moved -= _enemyView.Move;
        _monster.Movement.OnDirectionChange -= _enemyView.Flip;
        _monster.AnimationStateChanged -= _enemyView.SetAnimationState;
        _enemyView.DamageTaken -= _monster.Health.TakeDamage;
        _monster.Health.Hurt -= _enemyView.Hurt;
        _monster.Health.Dying -= _enemyView.Die;
    }
}