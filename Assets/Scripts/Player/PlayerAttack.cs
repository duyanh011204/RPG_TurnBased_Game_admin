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

    void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<TopDownMovement>();

        if (hitboxObject != null)
            hitboxObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canAttack)
            StartCoroutine(PerformAttack());
    }

    private IEnumerator PerformAttack()
    {
        canAttack = false;
        animator.SetTrigger("Attack");

        animator.SetFloat("LastHorizontal", movement.GetMovementDirection().x);
        animator.SetFloat("LastVertical", movement.GetMovementDirection().y);

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
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
