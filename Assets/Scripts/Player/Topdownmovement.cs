using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private bool usePixelSnapping = false;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 lastMovement;
    private Vector2 lastMoveDir = Vector2.down;
    private bool isAttacking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (animator == null)
            animator = GetComponent<Animator>();
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!isAttacking)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            if (movement.magnitude > 1)
                movement.Normalize();

            if (movement != Vector2.zero)
                lastMovement = movement;

            UpdateAnimation();
        }

        if (Input.GetMouseButtonDown(0) && !isAttacking)
            StartCoroutine(HandleAttack());

      

    }

    private IEnumerator HandleAttack()
    {
        isAttacking = true;
        animator.SetBool("IsMoving", false);
        animator.SetTrigger("Attack");

        animator.SetFloat("LastHorizontal", lastMovement.x);
        animator.SetFloat("LastVertical", lastMovement.y);

        yield return new WaitForSeconds(0.4f);
        isAttacking = false;
    }

    void FixedUpdate()
    {
        if (!isAttacking)
            rb.velocity = movement * moveSpeed;
        else
            rb.velocity = Vector2.zero;

        if (usePixelSnapping)
        {
            Vector3 pos = transform.position;
            pos.x = Mathf.Round(pos.x * 32f) / 32f;
            pos.y = Mathf.Round(pos.y * 32f) / 32f;
            transform.position = pos;
        }
    }

    void UpdateAnimation()
    {
        if (animator != null)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.magnitude);

            animator.SetFloat("LastHorizontal", lastMovement.x);
            animator.SetFloat("LastVertical", lastMovement.y);
        }

        if (spriteRenderer != null && movement.x != 0)
            spriteRenderer.flipX = movement.x < 0;
    }

    public void SetSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public Vector2 GetMovementDirection()
    {
        return movement;
    }

    public bool IsMoving()
    {
        return movement.magnitude > 0.1f;
    }
}
