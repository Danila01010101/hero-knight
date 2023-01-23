using System;
using UnityEngine;

namespace Model
{
    public class Transform
    {
        private float _movementSpeed;
        private Vector2 _position;
        private Directions _currentFaceDirection = Directions.Right;
        private enum Directions { Right, Left }

        public Vector2 CurrentPosition { get { return _position; } }

        public Action OnDirectionChange;
        public Func<Vector2, Vector2> Moved;

        public virtual void Move(Vector2 newMoveDirection)
        {
            Directions newFaceDirection = _currentFaceDirection;

            if (newMoveDirection.x > 0)
                newFaceDirection = Directions.Right;
            if (newMoveDirection.x < 0)
                newFaceDirection = Directions.Left;
            if (newFaceDirection != _currentFaceDirection)
            {
                OnDirectionChange?.Invoke();
                _currentFaceDirection = newFaceDirection;
            }
            var direction = Vector2.zero;
            direction.x = newMoveDirection.x * _movementSpeed;

            if (Moved != null)
                _position = Moved(direction);
        }

        public Transform(Vector2 currentPosition, float movementSpeed)
        {
            _position = currentPosition;
            _movementSpeed = movementSpeed;
        }
    }
}