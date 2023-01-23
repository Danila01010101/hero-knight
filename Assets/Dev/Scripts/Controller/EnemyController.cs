using Model;
using UnityEngine;
using View;

namespace Controller
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private EnemyView _enemyView;
        [SerializeField] private float _movementSpeed = 2f;

        private Enemy _enemy;

        private void Awake()
        {
            _enemy = new Enemy(_enemyView.transform.position, _movementSpeed);
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
            _enemy.Transform.Moved += _enemyView.SetMoveDirection;
            _enemy.Transform.OnDirectionChange += _enemyView.Flip;
            _enemy.AnimationStateChanged += _enemyView.SetAnimationState;
        }

        private void OnDisable()
        {
            _enemy.Transform.Moved -= _enemyView.SetMoveDirection;
            _enemy.Transform.OnDirectionChange -= _enemyView.Flip;
            _enemy.AnimationStateChanged -= _enemyView.SetAnimationState;
        }
    }
}