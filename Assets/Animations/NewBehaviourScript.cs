using UnityEngine;
public class SlimeIdle : MonoBehaviour
{
    void Update()
    {
        float scaleY = 1 + Mathf.Sin(Time.time * 3f) * 0.05f;
        transform.localScale = new Vector3(1.5f, scaleY, 1.5f);
    }
}
