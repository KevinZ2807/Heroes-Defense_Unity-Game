using UnityEngine;

public class SniperHero : BaseHero
{
    [Header("Attributes")]
    [SerializeField] private GameObject Sniper;

    void Update()
    {
        timeUntilFire += Time.deltaTime;
        FindTarget();
        if (target == null) {
            return;
        }
        if (!CheckTargetIsInRange()) { // Kiem tra neu co muc tieu vao vung ban'
            target = null;
        } else {
            Vector3 directionToTarget = target.position - Sniper.transform.position;
            float angle = (Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg);
            if (!m_FacingRight) {
                if (angle > 90) angle -= 180;
                if (angle < -90) angle += 180;
            }
            
            angle = Mathf.Clamp(angle, -90, 90);
            Sniper.transform.rotation = Quaternion.Euler(0, 0, angle);
            FacingTarget();
            if (timeUntilFire >= 1f / attackSpeed) {
                ShootTheGun();
            }
        }
    }
    
    private void ShootTheGun(){
        anim.Stop();
        animatorControl.Play("Attack");
        audioCue.Play();
        target.gameObject.GetComponent<BaseEnemy>().TakeDamage(attackDamage);
        timeUntilFire = 0f;
    }
}
