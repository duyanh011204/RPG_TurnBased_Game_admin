using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCamera : MonoBehaviour
{
    public Transform player;       // nhân vật của bạn
    public Transform enemy;        // kẻ địch (lock-on target)
    public Vector3 offset = new Vector3(0, 5, -8);
    public float smoothSpeed = 0.125f;

    void LateUpdate()
    {
        if (player == null) return;

        // Camera follow player
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Nếu có enemy → camera nhìn giữa Player và Enemy
        if (enemy != null)
        {
            Vector3 lookPoint = (player.position + enemy.position) / 2f;
            transform.LookAt(lookPoint);
        }
        else
        {
            // Nếu chưa có enemy thì chỉ nhìn player
            transform.LookAt(player);
        }
    }
}
