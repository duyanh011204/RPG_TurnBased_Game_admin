using UnityEngine;

public class CombatCameraOrbit : MonoBehaviour
{
    [Header("Target")]
    public Transform pivot; // Điểm giữa arena

    [Header("Camera Distance")]
    public float distance = 12f;
    public float height = 8f;
    public float minDistance = 8f;
    public float maxDistance = 20f;

    [Header("Rotation")]
    public float rotationSpeed = 100f;
    public float verticalAngle = 25f; // Góc nhìn xuống
    public float minVerticalAngle = 10f;
    public float maxVerticalAngle = 60f;

    [Header("Zoom")]
    public float zoomSpeed = 5f;

    [Header("Smoothing")]
    public float rotationSmoothing = 10f;

    private float currentHorizontalAngle = 0f;
    private float currentVerticalAngle = 25f;
    private float targetHorizontalAngle = 0f;
    private float targetVerticalAngle = 25f;
    private float currentDistance = 12f;

    void Start()
    {
        if (pivot == null)
        {
            GameObject pivotObj = new GameObject("CameraPivot");
            pivotObj.transform.position = new Vector3(0, 1.5f, 0);
            pivot = pivotObj.transform;
        }

        currentHorizontalAngle = transform.eulerAngles.y;
        currentVerticalAngle = verticalAngle;
        currentDistance = distance;
    }

    void LateUpdate()
    {
        HandleInput();
        UpdateCameraPosition();
    }

    void HandleInput()
    {
        // Giữ chuột phải để xoay (như Arcane Lineage)
        if (Input.GetMouseButton(0)) // Right mouse button
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            targetHorizontalAngle += mouseX * rotationSpeed * Time.deltaTime;
            targetVerticalAngle -= mouseY * rotationSpeed * Time.deltaTime;

            // Giới hạn góc dọc
            targetVerticalAngle = Mathf.Clamp(targetVerticalAngle, minVerticalAngle, maxVerticalAngle);
        }

        // Zoom bằng scroll wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            currentDistance -= scroll * zoomSpeed;
            currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);
        }

        // Reset camera (phím R)
        if (Input.GetKeyDown(KeyCode.R))
        {
            targetHorizontalAngle = 0f;
            targetVerticalAngle = 25f;
        }
    }

    void UpdateCameraPosition()
    {
        // Smooth rotation
        currentHorizontalAngle = Mathf.Lerp(currentHorizontalAngle, targetHorizontalAngle,
            rotationSmoothing * Time.deltaTime);
        currentVerticalAngle = Mathf.Lerp(currentVerticalAngle, targetVerticalAngle,
            rotationSmoothing * Time.deltaTime);

        // Tính toán vị trí camera
        Quaternion rotation = Quaternion.Euler(currentVerticalAngle, currentHorizontalAngle, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -currentDistance);

        transform.position = pivot.position + offset;
        transform.LookAt(pivot.position + Vector3.up * 0.5f);
    }

    // Optional: Focus vào target cụ thể (player hoặc enemy khi attack)
    public void FocusOnTarget(Transform target, float duration = 0.5f)
    {
        StartCoroutine(FocusCoroutine(target, duration));
    }

    System.Collections.IEnumerator FocusCoroutine(Transform target, float duration)
    {
        Vector3 originalPivot = pivot.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            pivot.position = Vector3.Lerp(originalPivot, target.position, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        // Return to center
        elapsed = 0f;
        while (elapsed < duration)
        {
            pivot.position = Vector3.Lerp(target.position, originalPivot, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}