using Model;
using UnityEngine;
using View;

namespace Controller
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private EnemyView _enemyView;
        [SerializeField] private float _movementSpeed = 2f;

        private BringerOfDeath _enemy;

        private void Awake()
        {
            _enemy = new BringerOfDeath(transform, _movementSpeed);
        }

        private void Start()
        {
            _enemy.Initialize();
        }

        private void Update()
        {
            _enemy.Update();
        }

        private void OnEnable()
        {
            _enemy.Movement.Moved += _enemyView.Move;
            _enemy.AttackStarted += _enemyView.Attack;
            _enemyView.EnemyLost += _enemy.LoseTarget;
            _enemyView.AttackEnded += _enemy.EndAttack;
            _enemy.Movement.OnDirectionChange += _enemyView.Flip;
            _enemyView.EnemyDetected += _enemy.DetectTargetToAttack;
            _enemy.AnimationStateChanged += _enemyView.SetAnimationState;
            _enemyView.DamageTaken += _enemy.Health.TakeDamage;
            _enemy.Health.Dying += _enemyView.Die;
        }

        private void OnDisable()
        {
            _enemy.Movement.Moved -= _enemyView.Move;
            _enemy.AttackStarted -= _enemyView.Attack;
            _enemyView.EnemyLost -= _enemy.LoseTarget;
            _enemyView.AttackEnded -= _enemy.EndAttack;
            _enemy.Movement.OnDirectionChange -= _enemyView.Flip;
            _enemyView.EnemyDetected -= _enemy.DetectTargetToAttack;
            _enemy.AnimationStateChanged -= _enemyView.SetAnimationState;
            _enemyView.DamageTaken -= _enemy.Health.TakeDamage;
            _enemy.Health.Dying -= _enemyView.Die;
        }
    }
}