using UnityEngine;
using Model;
using View;

namespace Controller
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private PlayerView _playerView;

        [SerializeField] private float _playerMovementSpeed = 0.2f;
        [SerializeField] private float _jumpForce = 1.5f;

        private Player _player;

        private void Awake()
        {
            _player = new Player(_playerMovementSpeed, _jumpForce);
        }

        private void OnEnable()
        {
            _playerInput.OnPlayerMove += _player.Move;
            _player.CharacterMoved += _playerView.SetMoveDirection;
            _player.OnDirectionChange += _playerView.Flip;
            _player.CharacterJumped += _playerView.Jump;
            _playerView.GroundDetected += _player.DetectGround;
        }

        private void OnDisable()
        {
            _playerInput.OnPlayerMove -= _player.Move;
            _player.CharacterMoved -= _playerView.SetMoveDirection;
            _player.OnDirectionChange -= _playerView.Flip;
            _player.CharacterJumped -= _playerView.Jump;
            _playerView.GroundDetected -= _player.DetectGround;
        }
    }
}