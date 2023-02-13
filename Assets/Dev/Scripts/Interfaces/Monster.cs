using UnityEngine;

namespace Model
{
    public abstract class Monster
    {
        private protected float _moveSpeed = 2f;
        private protected float _attackRange = 2f;
        protected Movement _movement;
    }
}