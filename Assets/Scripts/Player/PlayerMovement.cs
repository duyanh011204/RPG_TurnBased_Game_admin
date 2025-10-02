using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;       // tốc độ di chuyển
    public float jumpHeight = 2f;      // độ cao khi nhảy
    public float gravity = -9.81f;     // trọng lực
    public float rotationSpeed = 10f;  // tốc độ xoay mặt nhân vật

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Kiểm tra nhân vật có đang chạm đất không
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // reset nhỏ để dính đất
        }

        // Input bàn phím WASD
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Tạo vector di chuyển theo hướng camera
        Vector3 move = new Vector3(x, 0, z).normalized;

        if (move.magnitude >= 0.1f)
        {
            // Lấy hướng xoay dựa theo camera
            float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationVelocity, rotationSpeed * Time.deltaTime);

            // Xoay nhân vật
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Di chuyển theo hướng đã xoay
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        }

        // Nhảy
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Áp dụng trọng lực
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private float rotationVelocity; // để tính xoay mượt
}
