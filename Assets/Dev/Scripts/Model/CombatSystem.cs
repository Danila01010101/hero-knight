using System;
using UnityEngine;

namespace Model
{
    public class CombatSystem
    {
        private Sword _sword;
        private float _lastHitTime;

        public Action Attack;

        public CombatSystem(Sword sword)
        {
            _sword = sword;
        }

        public void TryAttack()
        {
            if (_lastHitTime <= Time.time - _sword.Cooldown)
            {
                _lastHitTime = Time.time;
                Attack?.Invoke();
            }
        }
    }
}