using System;
using UnityEngine;

namespace Model
{
    public class Movement
    {
        private float _movementSpeed;
        private Directions _currentFaceDirection = Directions.Right;
        private enum Directions { Right, Left }

        public readonly Transform Transform;

        public Action OnDirectionChange;
        public Action<Vector2> Moved;

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

            Moved?.Invoke(direction);
        }

        public Movement(Transform transform, float movementSpeed)
        {
            Transform = transform;
            _movementSpeed = movementSpeed;
        }
    }
}