using UnityEngine;
using System.Collections;

public class PlayerStrikeUI : MonoBehaviour
{
    [Header("Strike Settings")]
    [SerializeField] private Transform enemyTarget;
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float stopDistance = 1.2f;
    [SerializeField] private float attackDamage = 20f;
    [SerializeField] private float manaCost = 5f;

    private Animator animator;
    private bool isStriking = false;
    private Vector3 originalPosition;
    private PlayerStats playerStats;
    private BattleManager battleManager;

    void Awake()
    {
        animator = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();
        battleManager = FindObjectOfType<BattleManager>();
    }

    // ✅ Hàm công khai cho BattleManager gán enemy target
    public void SetEnemyTarget(Transform target)
    {
        enemyTarget = target;
    }

    public void OnStrikeButton()
    {
        if (isStriking || enemyTarget == null) return;

        if (playerStats != null && playerStats.currentMP < manaCost)
        {
            Debug.Log("Not enough mana!");
            return;
        }

        StartCoroutine(StrikeSequence());
    }

    private IEnumerator StrikeSequence()
    {
        isStriking = true;
        originalPosition = transform.position;

        if (playerStats != null)
            playerStats.currentMP -= manaCost;


        if (enemyTarget != null)
        {
            Vector3 lookPos = enemyTarget.position;
            lookPos.y = transform.position.y;
            transform.LookAt(lookPos);
        }

        // 1️⃣ Lao đến gần đối thủ (chỉ di chuyển trên mặt phẳng XZ)
        while (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z),
                                new Vector3(enemyTarget.position.x, 0, enemyTarget.position.z)) > stopDistance)
        {
            Vector3 targetPos = new Vector3(enemyTarget.position.x, transform.position.y, enemyTarget.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // 2️⃣ Bắt đầu animation Strike
        if (animator != null)
            animator.SetTrigger("Strike");


        // 3️⃣ Chờ một phần animation rồi gây sát thương
        float animLength = 1f;
        if (animator != null && animator.runtimeAnimatorController != null)
        {
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            foreach (var clip in clips)
            {
                if (clip.name == "Strike")
                {
                    animLength = clip.length;
                    break;
                }
            }
        }

        yield return new WaitForSeconds(animLength * 0.5f);

        // 4️⃣ Gây sát thương
        EnemyAI3D enemy = enemyTarget.GetComponent<EnemyAI3D>();
        if (enemy != null)
        {
            enemy.TakeDamage(attackDamage);
            
        }

        yield return new WaitForSeconds(animLength * 1f);

        // ⭐ Quay lại chỗ cũ và hướng cũ
        transform.LookAt(originalPosition);

        // 5️⃣ Quay lại vị trí ban đầu
        while (Vector3.Distance(transform.position, originalPosition) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // 6️⃣ Cập nhật UI & kết thúc lượt
        if (battleManager != null)
        {
            battleManager.UpdateUI();
            battleManager.EndTurn();
        }

        isStriking = false;
    }
}
