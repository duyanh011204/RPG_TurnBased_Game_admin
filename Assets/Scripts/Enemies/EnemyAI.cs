using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float patrolRadius = 3f;
    public float detectRadius = 5f;
    public float chaseSpeed = 3f;
    public float patrolSpeed = 1.5f;
    public float respawnTime = 10f;

    private Vector3 startPos;
    private Vector3 patrolTarget;
    private bool chasing;
    private bool returning;
    private bool dead;

    void Start()
    {
        startPos = transform.position;
        SetNewPatrolTarget();
    }

    void Update()
    {
        if (dead) return;

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist <= detectRadius)
        {
            chasing = true;
            returning = false;
        }
        else if (chasing && dist > detectRadius * 1.5f)
        {
            chasing = false;
            returning = true;
        }

        if (chasing) MoveTowards(player.position, chaseSpeed);
        else if (returning)
        {
            MoveTowards(startPos, patrolSpeed);
            if (Vector2.Distance(transform.position, startPos) < 0.2f)
            {
                returning = false;
                SetNewPatrolTarget();
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, patrolTarget) < 0.2f)
                SetNewPatrolTarget();
            MoveTowards(patrolTarget, patrolSpeed);
        }
    }

    void MoveTowards(Vector3 target, float speed)
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    void SetNewPatrolTarget()
    {
        patrolTarget = startPos + (Vector3)(Random.insideUnitCircle * patrolRadius);
    }

    public void Die()
    {
        if (dead) return;
        dead = true;
        StartCoroutine(Respawn());
        gameObject.SetActive(false);
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);
        transform.position = startPos;
        dead = false;
        gameObject.SetActive(true);
    }

    public void OnPlayerCollision(bool playerFirstStrike)
    {
        if (dead) return;
        CombatManager.Instance.StartEncounter(this, playerFirstStrike);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            OnPlayerCollision(false);
    }
}
