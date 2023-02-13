using System;
using UnityEngine;

namespace View
{
    public class PlayerInput : MonoBehaviour
    {
        public Action<Vector2> OnPlayerMove;
        public Action Attack;

        private MoveCommand _moveCommand;

        private void Start()
        {
            _moveCommand = new MoveCommand(OnPlayerMove);
        }

        private void Update()
        {
            CheckMoveInput();
        }

        private void CheckMoveInput()
        {
            if (Input.GetKey(KeyCode.W)) _moveCommand.AddDirection(MoveCommand.Direction.Up);
            if (Input.GetKey(KeyCode.S)) _moveCommand.AddDirection(MoveCommand.Direction.Down);
            if (Input.GetKey(KeyCode.D)) _moveCommand.AddDirection(MoveCommand.Direction.Right);
            if (Input.GetKey(KeyCode.A)) _moveCommand.AddDirection(MoveCommand.Direction.Left);

            _moveCommand.Execute();

            if (Input.GetKey(KeyCode.Space)) Attack?.Invoke();
        }
    }
}