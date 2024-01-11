using UnityEngine;

public class PoisonBullet : Bullet
{
    // Start is called before the first frame update
    private void Awake()
    {
        timeBeforeDestroy = 2f;
        Destroy(gameObject, timeBeforeDestroy);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        // TAKE Health from enemy
        if (other.gameObject.layer == LayerMask.NameToLayer("AirEnemy")) return;
        other.gameObject.GetComponent<BaseEnemy>().TakeDamage(GetBulletDamage());
        if (other.gameObject.GetComponent<PoisonEffect>() == null)
        {
            // Add the poison effect to the enemy
            other.gameObject.AddComponent<PoisonEffect>();
        }

        Destroy(gameObject);
    }
}
