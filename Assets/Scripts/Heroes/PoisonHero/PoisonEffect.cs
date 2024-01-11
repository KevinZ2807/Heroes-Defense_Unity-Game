using System.Collections;
using UnityEngine;

public class PoisonEffect : MonoBehaviour
{
    public int damagePerSecond = 2;
    public float duration = 6.0f; 

    private float timer;

    void Start()
    {
        StartCoroutine(ApplyPoison());
    }

    IEnumerator ApplyPoison()
    {
        while (duration > 0)
        {
            duration -= Time.deltaTime;
            timer += Time.deltaTime;

            if (timer >= 1f)
            {
                ApplyDamage(damagePerSecond);
                timer = 0;
            }

            yield return null;
        }

        // Optionally destroy the poison effect or disable it
        Destroy(this);
    }

    void ApplyDamage(int damage)
    {
        gameObject.GetComponent<BaseEnemy>().TakeDamage(damage);
    }
}
