using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private bool usePixelSnapping = false; // Bật nếu muốn snap vào grid

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 lastMovement; // Lưu hướng cuối để idle đúng hướng

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Setup Rigidbody2D nếu chưa có
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        rb.gravityScale = 0f; // Top-down không dùng gravity
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Không xoay

        // Get components nếu chưa assign
        if (animator == null)
            animator = GetComponent<Animator>();
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Input
        movement.x = Input.GetAxisRaw("Horizontal"); // A/D hoặc Left/Right
        movement.y = Input.GetAxisRaw("Vertical");   // W/S hoặc Up/Down
       

        // Normalize để diagonal không nhanh hơn
        if (movement.magnitude > 1)
        {
            movement.Normalize();
        }

        // Lưu hướng nếu đang di chuyển
        if (movement != Vector2.zero)
        {
            lastMovement = movement;
        }

        // Update Animation
        UpdateAnimation();
    }

    void FixedUpdate()
    {
        // Di chuyển
        rb.velocity = movement * moveSpeed;

        // Pixel snapping (optional)
        if (usePixelSnapping)
        {
            Vector3 pos = transform.position;
            pos.x = Mathf.Round(pos.x * 32f) / 32f; // 32 = pixels per unit
            pos.y = Mathf.Round(pos.y * 32f) / 32f;
            transform.position = pos;
        }
    }

    void UpdateAnimation()
    {
        if (animator != null)
        {
            // Set parameters cho Animator
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.magnitude);

            // Idle direction
            animator.SetFloat("LastHorizontal", lastMovement.x);
            animator.SetFloat("LastVertical", lastMovement.y);
        }

        // Flip sprite theo hướng (nếu không dùng animation)
        if (spriteRenderer != null && movement.x != 0)
        {
            spriteRenderer.flipX = movement.x < 0;
        }
    }

    // Public methods để code khác gọi
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