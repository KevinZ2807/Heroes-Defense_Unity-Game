using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BaseEnemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Slider healthBar;
    [SerializeField] protected Animator anim;
    [SerializeField] private AudioSource audioPlay;
    [Header("Attributes")]
    [SerializeField] protected bool isBoss = false;
    [SerializeField] protected int health = 2;
    [SerializeField] protected int damage = 10;
    [SerializeField] protected int incomeWorth = 50;
    [SerializeField] protected bool isFly = false;
    [SerializeField] protected float moveSpeed = 2f; // SerializeField: Private but still appear in UI Unity
    [SerializeField] private bool isDead = false;
    
    // Start is called before the first frame update
    private Transform _target;
    private float _baseSpeed;
    private bool _isDestroyed = false;
    private int currentHealth;
    private float _timeFromSpawn;
    private int _gateIndex;
    public int cnt = 0;

    protected virtual void Start() {
        // Change layer to "Enemy"
        int LayerIgnoreRaycast = LayerMask.NameToLayer("Enemy");
        if (isFly) {
            LayerIgnoreRaycast = LayerMask.NameToLayer("AirEnemy");
        }
        gameObject.layer = LayerIgnoreRaycast;
        _timeFromSpawn = 0;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        audioPlay = GetComponent<AudioSource>();

        currentHealth = health;
        _baseSpeed = moveSpeed;
        UpdateHealthBar();
    }

    // Movement
    protected virtual void Update() {
        _timeFromSpawn += Time.deltaTime;
        if (Vector2.Distance(_target.position, transform.position) <= 0.1f) {

            MapEventHandler position = _target.GetComponent<MapEventHandler>();

            if (position.row == 19 && position.col > 6 && position.col < 13) { // Khi enemy vuot qua diem cuoi cung
                GameManager.Ins.UpdateHealth(Mathf.Abs(damage));
                StartCoroutine(Die(true));
                return;
            } else {
                Transform _newTarget = GameEngine.Ins.NextPath(position.row, position.col, _timeFromSpawn,cnt)?.transform;
                if (!_newTarget)
                {
                    Debug.Log("null " + position.row + " " + position.col);
                }
                // Enemy di tiep
                if (_target.position.x > _newTarget.position.x)
                {
                    transform.eulerAngles = Vector3.up * 180;
                }
                else
                {
                    transform.eulerAngles = Vector3.zero;
                }
                _target = _newTarget;
            }
        }
        
    }

    private void FixedUpdate() {
        Vector2 direction = (_target.position - transform.position).normalized; // normalized: gioi han gia tri tu 0 den 1
        
        rb.velocity = direction * moveSpeed; // Enemy se di theo huong tiep theo
    }
    
   public void UpdateSpeed(float newSpeed) {
        if (moveSpeed != _baseSpeed) return;
        moveSpeed =  moveSpeed * newSpeed;
        Debug.Log(moveSpeed);
    }
    public void ResetSpeed() {
        moveSpeed = _baseSpeed;
    }
    public bool GetIsDead() {
        return isDead;
    }

    public void SetGateIndex(int index)
    {
        _gateIndex = index;
        _target = GameEngine.Ins.NextPath(0, 6 + index + 1, _timeFromSpawn,cnt).transform;
    }

    public IEnumerator Die(bool instantly)
    {
        isDead = true;
        anim.SetBool("isDead", true);
        UpdateSpeed(0f);
        AudioController.Ins.PlayDestroySFX();
        EnemySpawner.onEnemyDestroy.Invoke();
        if (!instantly)
        {
            yield return new WaitForSeconds(1f);
        }
        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        audioPlay.Play();
        UpdateHealthBar();

        if (currentHealth <= 0 && !_isDestroyed) {
            GameManager.Ins.AddCurrency(incomeWorth);
            // If many bullet hit an enemy at the same time, number of enemies will be minus to negative
            _isDestroyed = true; // So this will solve the issue
            StartCoroutine(Die(false));
        }
    }
    private void UpdateHealthBar()
    {
            healthBar.value = (float)(currentHealth * 1.0 / health);
    }
    public void TakeSpecialSkill(BaseHero Hero)
    {
        
    }
}
