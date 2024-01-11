using UnityEngine;

public class Arrow : Bullet
{
    private void Awake() {
        timeBeforeDestroy = 2f;
        Destroy(gameObject, timeBeforeDestroy);
    }
    
    private void OnCollisionEnter2D(Collision2D other) {
        // TAKE Health from enemy
        other.gameObject.GetComponent<BaseEnemy>().TakeDamage(GetBulletDamage());
        
        Destroy(gameObject);
    }
}
