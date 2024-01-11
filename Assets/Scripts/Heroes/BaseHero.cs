using UnityEngine.UI;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Linq;
public class BaseHero : MonoBehaviour
{
    [Header("References")]
    public Animation anim;
    public Animator animatorControl;
    public AudioSource audioCue;
    [SerializeField] protected LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject indicator;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private GameObject sellingUI;
    [SerializeField] private Button sellingButton;
    [Header("Attribute")]
    [SerializeField] protected float attackRange = 5f;
    [SerializeField] protected float attackSpeed = 1f; // Bullet per second
    [SerializeField] protected int attackDamage = 1;
    [SerializeField] private int price;
    //[SerializeField] private bool canAir = false;
    protected Transform target;
    protected float timeUntilFire = 1000;
    protected bool m_FacingRight = true;

    public Sprite sprite;

    /*private void OnDrawGizmosSelected() { // Ve vong tron xung quanh hero (range)
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, attackRange);
    } */
    void Start()
    {
        anim = GetComponent<Animation>();
        animatorControl = GetComponent<Animator>();
        audioCue = GetComponent<AudioSource>();
        sellingButton.onClick.AddListener(Sell);

        indicator.transform.localScale = new Vector3(attackRange, attackRange, 1);
        indicator.SetActive(false);
    }

    // Update is called once per frame
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
            Attack();
        }
    }

    public void Attack()
    {
        if (timeUntilFire >= 1f / attackSpeed) {
            anim.Stop();
            animatorControl.Play("Attack");
            audioCue.Play();
            FacingTarget();
            Shoot();
            timeUntilFire = 0f;
        }
    }

    protected void FacingTarget() {
        Vector2 position = transform.position;
        if (position.x >= target.position.x && m_FacingRight) { // Neu dich ben trai
            FlipHorizontal();
            m_FacingRight = false;
            Debug.Log("Looking left");
        }
        if (position.x < target.position.x && !m_FacingRight) {
            FlipHorizontal();
            m_FacingRight = true;
            Debug.Log("Looking right");
        }
        
    }

    private void Shoot() {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(target);
        bulletScript.RotateBullet();
        Debug.Log("Shoot");
    }

    protected bool CheckTargetIsInRange() {
        return Vector2.Distance(target.position, transform.position) <= attackRange; 
    }

    protected void FindTarget() {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, attackRange, 
        (Vector2) transform.position, 0f, enemyMask);

        if (hits.Length > 0) { // When we hit something
            var closestEnemy = hits.Select(hit => hit.transform) // Tim enemy gan nhat de ban
                               .Where(t => !t.GetComponent<BaseEnemy>().GetIsDead())
                               .OrderBy(t => Vector2.Distance(transform.position, t.position))
                               .FirstOrDefault();
        
            if (closestEnemy != null) {
                target = closestEnemy;
            }
        }
    }
    private void FlipHorizontal() {
		// Switch the way the player is labelled as facing.

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
		//transform.Rotate (0f, 180f, 0f);
	}
    public void OpenSellingUI() {
        sellingUI.SetActive(true);
    }
    public void CloseSellingUI() {
        sellingUI.SetActive(false);
        UIManager.main.SetHoveringState(false); // Thoat khoi va co the dat hero lai
    }

    public void Sell() {
        CloseSellingUI();
        Destroy(gameObject);
        GameManager.Ins.AddCurrency(Mathf.RoundToInt(price/2));
    }

    public void SetAttackSpeed(float value) {
        attackSpeed = value;
    }
    public void SetAttackRange(float value) {
        attackRange = value;
    }

    public int GetPrice() {
        return price;
    }

    void OnMouseEnter()
    {
        // Show the range indicator when the mouse hovers over the tower
        if (BuildManager.main.isSelectedHero) return;
        Debug.Log("Mouse touched the tower");
        indicator.SetActive(true);
    }

    void OnMouseExit()
    {
        if (BuildManager.main.isSelectedHero) return;
        Debug.Log("Mouse escaped the tower");
        indicator.SetActive(false);
    }

    
}
