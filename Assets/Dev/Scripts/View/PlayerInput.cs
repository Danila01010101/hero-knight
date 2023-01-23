using System;
using UnityEngine;

namespace View
{
    public class PlayerInput : MonoBehaviour
    {
        public Action<Vector2> OnPlayerMove;

        private void Update()
        {
            CheckMoveInput();
        }

        private void CheckMoveInput()
        {
            Vector2 directionToMove = new Vector2();

            if (Input.GetKey(KeyCode.W)) directionToMove.y += 1;
            if (Input.GetKey(KeyCode.S)) directionToMove.y -= 1;
            if (Input.GetKey(KeyCode.D)) directionToMove.x += 1;
            if (Input.GetKey(KeyCode.A)) directionToMove.x -= 1;

            OnPlayerMove?.Invoke(directionToMove);
        }
    }
}