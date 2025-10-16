using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float maxHealth = 50f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float detectionRange = 5f;

    [Header("References")]
    [SerializeField] private Transform target;
    private Rigidbody2D rb;
    private Animator animator;

    private float currentHealth;
    private bool isDead = false;
    private bool encounterTriggered = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        if (target == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                target = playerObj.transform;
        }
    }

    void Update()
    {
        if (isDead || target == null || encounterTriggered) return;

        float distance = Vector2.Distance(transform.position, target.position);
        if (distance <= detectionRange)
            ChasePlayer();
        else
            rb.velocity = Vector2.zero;
    }

    void ChasePlayer()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;

        if (animator != null)
        {
            animator.SetFloat("Horizontal", direction.x);
            animator.SetFloat("Vertical", direction.y);
            animator.SetFloat("Speed", rb.velocity.magnitude);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead || encounterTriggered) return;
        if (other.CompareTag("Player"))
        {
            encounterTriggered = true;
            rb.velocity = Vector2.zero;
            if (animator != null)
                animator.SetFloat("Speed", 0);
            EncounterManager.Instance.StartEncounter(false);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (animator != null)
            animator.SetTrigger("Hit");

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        isDead = true;
        rb.velocity = Vector2.zero;

        if (animator != null)
            animator.SetTrigger("Die");

        Destroy(gameObject, 0.5f);
    }
}
