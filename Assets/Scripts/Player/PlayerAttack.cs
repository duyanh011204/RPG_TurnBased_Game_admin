using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackDamage = 20f;
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private GameObject hitboxObject;

    private bool canAttack = true;
    private Animator animator;
    private TopDownMovement movement;
    private Vector2 lastMoveDir = Vector2.down;

    void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<TopDownMovement>();

        if (hitboxObject != null)
            hitboxObject.SetActive(false);
    }

    void Update()
    {
        Vector2 moveDir = movement.GetMovementDirection();
        if (moveDir != Vector2.zero)
            lastMoveDir = moveDir;

        if (Input.GetMouseButtonDown(0) && canAttack)
            StartCoroutine(PerformAttack());
    }

    private IEnumerator PerformAttack()
    {
        canAttack = false;
        animator.SetTrigger("Attack");

        animator.SetFloat("LastHorizontal", lastMoveDir.x);
        animator.SetFloat("LastVertical", lastMoveDir.y);

        PositionHitbox();
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    void PositionHitbox()
    {
        if (hitboxObject == null)
            return;

        Vector3 offset = Vector3.zero;

        if (lastMoveDir == Vector2.up)
            offset = new Vector3(0, 0.4f);
        else if (lastMoveDir == Vector2.down)
            offset = new Vector3(0, -0.4f);
        else if (lastMoveDir == Vector2.left)
            offset = new Vector3(-0.4f, 0);
        else if (lastMoveDir == Vector2.right)
            offset = new Vector3(0.4f, 0);

        hitboxObject.transform.localPosition = offset;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyAttack"))
        {
            if (BattleStartData.TryStartBattle(false))
            {
                SceneTransitions.LoadScene("CombatScene");
            }
        }
    }
    public void EnableHitbox()
    {
        if (hitboxObject != null)
            hitboxObject.SetActive(true);
    }

    public void DisableHitbox()
    {
        if (hitboxObject != null)
            hitboxObject.SetActive(false);
    }

    public float GetAttackDamage()
    {
        return attackDamage;
    }
}
