using UnityEngine;
using Model;
using View;

namespace Controller
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private PlayerView _playerView;

        [SerializeField] private Sword _sword;
        [SerializeField] private float _playerMovementSpeed = 0.2f;
        [SerializeField] private float _jumpForce = 1.5f;

        private Player _player;

        private void Awake()
        {
            _player = new Player(transform, _sword, _playerMovementSpeed, _jumpForce);
        }

        private void OnEnable()
        {
            _playerInput.OnPlayerMove += _player.Transform.Move;
            _player.Transform.Moved += _playerView.DetectInput;
            _player.Transform.OnDirectionChange += _playerView.Flip;
            _player.Transform.CharacterJumped += _playerView.Jump;
            _playerView.DamageDetected += _player.Health.TakeDamage;
            _player.Health.Hurt += _playerView.TakeDamage;
            _player.Health.Dying += _playerView.Die;
            _player.Health.Dying += _playerInput.Disable;
            _playerInput.Attack += _player.CombatSystem.TryAttack;
            _player.CombatSystem.Attack += _playerView.Attack;
        }

        private void OnDisable()
        {
            _playerInput.OnPlayerMove -= _player.Transform.Move;
            _player.Transform.Moved -= _playerView.DetectInput;
            _player.Transform.OnDirectionChange -= _playerView.Flip;
            _player.Transform.CharacterJumped -= _playerView.Jump;
            _playerView.DamageDetected -= _player.Health.TakeDamage;
            _player.Health.Hurt -= _playerView.TakeDamage;
            _player.Health.Dying -= _playerView.Die;
            _player.Health.Dying -= _playerInput.Disable;
            _playerInput.Attack -= _player.CombatSystem.TryAttack;
            _player.CombatSystem.Attack -= _playerView.Attack;
        }
    }
}