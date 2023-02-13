using System;

namespace Model
{
    public class Health
    {
        private int _value = 15;
        private bool _isAlive = true;

        public bool IsAlive => _isAlive;
        public Action Hurt;
        public Action Dying;

        public void TakeDamage(int value)
        {
            if (!_isAlive) return;

            _value -= value;
            Hurt?.Invoke();
            if (_value <= 0)
            {
                _value = 0;
                _isAlive = false;
                Dying?.Invoke();
            }
        }
    }
}