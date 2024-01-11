using UnityEngine;

public class MeleeHero : BaseHero
{
    void Update()
    {
        timeUntilFire += Time.deltaTime;
        if (target == null) {
            FindTarget();
            return;
        }
        if (!CheckTargetIsInRange()) { // Kiem tra neu co muc tieu vao vung ban'
            target = null;
        } else {
            MeleeAttack();
        }
    }

    private void MeleeAttack() {
        if (timeUntilFire >= 1f / attackSpeed) {
            anim.Stop();
            animatorControl.Play("Attack");
            audioCue.Play();
            FacingTarget();
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, attackRange, 
            (Vector2) transform.position, 0f, enemyMask);
            
            for (int i = 0; i < hits.Length; i++) {
                RaycastHit2D hit = hits[i];

                BaseEnemy be = hit.transform.GetComponent<BaseEnemy>();
                be.TakeDamage(attackDamage);
            }
            timeUntilFire = 0f;
        }
    }
}
