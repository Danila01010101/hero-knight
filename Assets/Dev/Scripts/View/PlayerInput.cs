using System;
using UnityEngine;

namespace View
{
    public class PlayerInput : MonoBehaviour
    {
        public Action<Vector2> OnPlayerMove;
        public Action Attack;

        private JoystickInputAsset _joystickInput;

        private void Awake()
        {
            _joystickInput = new JoystickInputAsset();
        }

        private void Start()
        {
            _joystickInput.Player.Attack.performed += context => Attack?.Invoke();
        }

        private void Update()
        {
            var newDirection = _joystickInput.Player.Move.ReadValue<Vector2>();
            OnPlayerMove?.Invoke(newDirection);
        }

        public void Disable()
        {
            GetComponent<PlayerInput>().enabled = false;
        }

        private void OnEnable()
        {
            _joystickInput.Enable();
        }

        private void OnDisable()
        {
            _joystickInput.Disable();
        }
    }
}