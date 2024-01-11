using System.Collections;
using UnityEngine;

public class IceHero : BaseHero
{
    [SerializeField] private float freezeTime = 4f;
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
           FreezeEnemy();
        }
    }

    private void FreezeEnemy() {
        if (timeUntilFire >= 1f / attackSpeed) {
            timeUntilFire = 0f;
            anim.Stop();
            animatorControl.Play("Attack");
            FacingTarget();
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, attackRange, 
            (Vector2) transform.position, 0f, enemyMask);
            
            for (int i = 0; i < hits.Length; i++) {
                RaycastHit2D hit = hits[i];

                BaseEnemy be = hit.transform.GetComponent<BaseEnemy>();
                be.UpdateSpeed(0.5f);
                be.TakeDamage(attackDamage);

                StartCoroutine(ResetEnemySpeed(be));
            }
        }
    }

    private IEnumerator ResetEnemySpeed(BaseEnemy be) {
        yield return new WaitForSeconds(freezeTime);

        be.ResetSpeed();
    }
}

