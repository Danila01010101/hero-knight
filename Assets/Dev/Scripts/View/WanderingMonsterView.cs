using UnityEngine;

public class WanderingMonsterView : MonsterView
{
    [SerializeField] private Vector2 _pushForce = new Vector2(1, 0.5f);
    [SerializeField] private int _damage = 5;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamagable target;
        PlayerView playerView;

        if (collision.gameObject.TryGetComponent(out target))
        {
            target.TakeDamage(_damage);
            if (collision.gameObject.TryGetComponent(out playerView))
            {
                Vector2 direction = (playerView.transform.position - transform.position).normalized;
                playerView.Push(_pushForce * direction);
            }
        }
    }
}
