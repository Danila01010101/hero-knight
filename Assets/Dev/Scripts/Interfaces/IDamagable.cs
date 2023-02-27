using UnityEngine;

public interface IDamagable
{
    public void TakeDamage(int value);
    public ParticleSystem GetDamageParticles();
}
