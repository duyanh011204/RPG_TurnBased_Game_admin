using UnityEngine;


public class EnemyAI2D : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float patrolRadius = 3f;
    [SerializeField] private float returnRangeMultiplier = 2f;

    [Header("Combat Stats")]
    [SerializeField] public float maxHP = 100f;
    public float currentHP;
    public bool isDead3D = false;
    public bool isDead2D = false;

    [Header("References")]
    [SerializeField] private Transform target;
    [SerializeField] private GameObject attackHitbox;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 spawnPoint;
    private Vector2 patrolTarget;
    private float lastAttackTime;

    private enum EnemyState { Idle, Patrol, Chase, Attack, Return }
    private EnemyState currentState;
    private float idleTimer;
    private float idleDuration = 2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        animator = GetComponent<Animator>();
        spawnPoint = transform.position;
        currentState = EnemyState.Idle;

        currentHP = maxHP;

        if (attackHitbox != null)
        {
            attackHitbox.SetActive(false);
            BoxCollider2D box = attackHitbox.GetComponent<BoxCollider2D>();
            if (box != null) box.isTrigger = true;
        }

        if (target == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) target = playerObj.transform;
        }
    }

    void Update()
    {
        if (isDead3D) return;

        float distanceToPlayer = target != null ? Vector2.Distance(transform.position, target.position) : Mathf.Infinity;
        float distanceFromSpawn = Vector2.Distance(transform.position, spawnPoint);

        switch (currentState)
        {
            case EnemyState.Idle:
                rb.velocity = Vector2.zero;
                idleTimer += Time.deltaTime;
                if (distanceToPlayer <= detectionRange && distanceFromSpawn <= detectionRange * returnRangeMultiplier)
                {
                    currentState = EnemyState.Chase;
                    break;
                }
                if (idleTimer >= idleDuration)
                {
                    idleTimer = 0f;
                    ChoosePatrolPoint();
                    currentState = EnemyState.Patrol;
                }
                break;

            case EnemyState.Patrol:
                MoveTowards(patrolTarget);
                if (Vector2.Distance(transform.position, patrolTarget) < 0.2f) currentState = EnemyState.Idle;
                if (distanceToPlayer <= detectionRange && distanceFromSpawn <= detectionRange * returnRangeMultiplier)
                    currentState = EnemyState.Chase;
                break;

            case EnemyState.Chase:
                if (target == null) break;
                if (distanceFromSpawn > detectionRange * returnRangeMultiplier)
                {
                    currentState = EnemyState.Return;
                    break;
                }
                if (distanceToPlayer <= attackRange)
                {
                    currentState = EnemyState.Attack;
                    break;
                }
                MoveTowards(target.position);
                break;

            case EnemyState.Attack:
                rb.velocity = Vector2.zero;
                if (Time.time - lastAttackTime >= attackCooldown)
                {
                    lastAttackTime = Time.time;
                    if (animator != null) animator.SetTrigger("Attack");
                }

                if (distanceToPlayer > attackRange && distanceToPlayer <= detectionRange)
                    currentState = EnemyState.Chase;
                else if (distanceToPlayer > detectionRange * returnRangeMultiplier)
                    currentState = EnemyState.Return;
                break;

            case EnemyState.Return:
                MoveTowards(spawnPoint);
                if (Vector2.Distance(transform.position, spawnPoint) < 0.2f) currentState = EnemyState.Idle;
                break;
        }

        UpdateAnimation();
    }

    void ChoosePatrolPoint()
    {
        Vector2 randomOffset = Random.insideUnitCircle * patrolRadius;
        patrolTarget = spawnPoint + randomOffset;
    }

    void MoveTowards(Vector2 targetPos)
    {
        Vector2 dir = (targetPos - (Vector2)transform.position).normalized;
        rb.velocity = dir * moveSpeed;
    }

    void UpdateAnimation()
    {
        if (animator == null) return;
        animator.SetFloat("Speed", rb.velocity.magnitude);
        if (rb.velocity.magnitude > 0.01f)
        {
            animator.SetFloat("Horizontal", rb.velocity.x);
            animator.SetFloat("Vertical", rb.velocity.y);
        }
    }

    public void EnableAttackHitbox()
    {
        if (attackHitbox != null)
            attackHitbox.SetActive(true);
    }

    public void DisableAttackHitbox()
    {
        if (attackHitbox != null)
            attackHitbox.SetActive(false);
    }

    // --- Combat ---
    public void TakeDamage(float damage)
    {
        if (isDead3D) return;

        currentHP -= damage;
        if (currentHP <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        isDead3D = true;
        if (animator != null)
            animator.SetTrigger("Die");

        rb.velocity = Vector2.zero;
        this.enabled = false;

        Destroy(gameObject, 1f);
    }
}