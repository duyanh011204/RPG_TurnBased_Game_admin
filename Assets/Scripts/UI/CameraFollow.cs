using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target; // Player
    
    [Header("Follow Settings")]
    [SerializeField] private float smoothSpeed = 5f; // Tốc độ follow (càng cao càng cứng)
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10); // Offset từ player
    
    [Header("Bounds (Optional)")]
    [SerializeField] private bool useBounds = false;
    [SerializeField] private Vector2 minBounds; // Góc trái dưới map
    [SerializeField] private Vector2 maxBounds; // Góc phải trên map
    
    [Header("Dead Zone (Optional)")]
    [SerializeField] private bool useDeadZone = false;
    [SerializeField] private float deadZoneWidth = 1f;
    [SerializeField] private float deadZoneHeight = 1f;
    
    private Camera cam;
    
    void Start()
    {
        cam = GetComponent<Camera>();
        
        // Tự tìm player nếu chưa assign
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
        }
    }
    
    void LateUpdate()
    {
        if (target == null) return;
        
        Vector3 desiredPosition;
        
        if (useDeadZone)
        {
            desiredPosition = GetDeadZonePosition();
        }
        else
        {
            desiredPosition = target.position + offset;
        }
        
     
        Vector3 smoothedPosition = Vector3.Lerp(
            transform.position, 
            desiredPosition, 
            smoothSpeed * Time.deltaTime
        );
        
      
        if (useBounds)
        {
            smoothedPosition = ClampToBounds(smoothedPosition);
        }
        
        transform.position = smoothedPosition;
    }
    
    Vector3 GetDeadZonePosition()
    {
        Vector3 currentPos = transform.position;
        Vector3 targetPos = target.position + offset;
        
     
        float deltaX = targetPos.x - currentPos.x;
        float deltaY = targetPos.y - currentPos.y;
        
        if (Mathf.Abs(deltaX) > deadZoneWidth / 2f)
        {
            currentPos.x = targetPos.x - (Mathf.Sign(deltaX) * deadZoneWidth / 2f);
        }
        
        if (Mathf.Abs(deltaY) > deadZoneHeight / 2f)
        {
            currentPos.y = targetPos.y - (Mathf.Sign(deltaY) * deadZoneHeight / 2f);
        }
        
        currentPos.z = targetPos.z;
        return currentPos;
    }
    
    Vector3 ClampToBounds(Vector3 position)
    {
     
        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;
     
        position.x = Mathf.Clamp(position.x, minBounds.x + camWidth, maxBounds.x - camWidth);
        position.y = Mathf.Clamp(position.y, minBounds.y + camHeight, maxBounds.y - camHeight);
        
        return position;
    }
    
    // Vẽ gizmos để debug
    void OnDrawGizmosSelected()
    {
        if (useBounds)
        {
            Gizmos.color = Color.red;
            Vector3 bottomLeft = new Vector3(minBounds.x, minBounds.y, 0);
            Vector3 topRight = new Vector3(maxBounds.x, maxBounds.y, 0);
            Vector3 bottomRight = new Vector3(maxBounds.x, minBounds.y, 0);
            Vector3 topLeft = new Vector3(minBounds.x, maxBounds.y, 0);
            
            Gizmos.DrawLine(bottomLeft, bottomRight);
            Gizmos.DrawLine(bottomRight, topRight);
            Gizmos.DrawLine(topRight, topLeft);
            Gizmos.DrawLine(topLeft, bottomLeft);
        }
        
        if (useDeadZone && target != null)
        {
            Gizmos.color = Color.yellow;
            Vector3 center = transform.position;
            center.z = 0;
            Gizmos.DrawWireCube(center, new Vector3(deadZoneWidth, deadZoneHeight, 0));
        }
    }

    
  
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
    
    public void SetSmoothSpeed(float speed)
    {
        smoothSpeed = speed;
    }
    
    public void SnapToTarget()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
}