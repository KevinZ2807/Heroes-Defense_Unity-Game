using UnityEngine;

public class IceBullet : Bullet
{
    private void Awake() {
        SetBulletDamage(1);
        SetIfFollow(true);
        Destroy(gameObject, timeBeforeDestroy);
    }
    
    private void OnCollisionEnter2D(Collision2D other) {
        // TAKE Health from enemy
        other.gameObject.GetComponent<BaseEnemy>().UpdateSpeed(0.25f);
        other.gameObject.GetComponent<BaseEnemy>().TakeDamage(GetBulletDamage());
        
        Destroy(gameObject);
    }
}
