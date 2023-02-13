using UnityEngine;

namespace Model
{
    public class Player
    {
        private Health _health;
        private PlayerMovement _movement;
        private CombatSystem _combatSystem;

        public Health Health => _health;
        public CombatSystem CombatSystem => _combatSystem;
        public PlayerMovement Transform { get { return _movement; } }

        public Player(Transform transform, Sword weapon, float movementSpeed, float jumpForce)
        {
            _movement = new PlayerMovement(transform, movementSpeed, jumpForce);
            _combatSystem = new CombatSystem(weapon);
            _health = new Health();

            _health.Dying += _movement.DisableMovement;
        }
    }
}