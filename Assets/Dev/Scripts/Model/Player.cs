using System;
using UnityEngine;

namespace Model
{
    public class Player
    {
        private PlayerTransform _transform;

        public PlayerTransform Transform { get { return _transform; } }

        public Player(Vector2 position, float movementSpeed, float jumpForce)
        {
            _transform = new PlayerTransform(position, movementSpeed, jumpForce);
        }
    }
}