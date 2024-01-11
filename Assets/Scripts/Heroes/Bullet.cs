using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private int bulletDamage = 2;
    [SerializeField] protected float timeBeforeDestroy = 5f;
    [SerializeField] private bool ifFollow = false;

    private Transform _target;
    private Vector2 directionToTarget;

    public void SetTarget(Transform theTarget) {
        _target = theTarget;
    }

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        Destroy(this, timeBeforeDestroy);
    }

    private void FixedUpdate() {
        if (!_target) return;

        // If bullet follow enemy
        if (ifFollow) {
            Vector2 direction = (_target.position - transform.position).normalized;
            rb.velocity = direction * bulletSpeed;
        } else {
            rb.velocity = directionToTarget * bulletSpeed;
        }
    }

    public void RotateBullet() {
        directionToTarget = (_target.position - transform.position).normalized;
        //float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }


    // Bullet Get/Set
    public void SetBulletDamage(int value) {
        bulletDamage = value;
    }

    public int GetBulletDamage() {
        return bulletDamage;
    }
    public void SetBulletSpeed(float value) {
        bulletSpeed = value;
    }

    public float GetBulletSpeed() {
        return bulletSpeed;
    }
    //

    // ifFollow Get/Set
    public void SetIfFollow(bool value) {
        ifFollow = value;
    }

    public bool GetIfFollow() {
        return ifFollow;
    }

    /*private void OnCollisionEnter2D(Collision2D other) {
        // TAKE Health from enemy
        other.gameObject.GetComponent<EnemyMovement>().UpdateSpeed(0.25f);
        other.gameObject.GetComponent<Enemy_Health>().TakeDamage(bulletDamage);
        
        Destroy(gameObject);
    }*/
}
