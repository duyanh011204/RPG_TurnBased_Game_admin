using UnityEngine;

public class BlinkEffect : MonoBehaviour
{
    [SerializeField] private float blinkSpeed = 1f;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    void Update()
    {
        float alpha = (Mathf.Sin(Time.time * blinkSpeed) + 1f) / 2f;
        canvasGroup.alpha = alpha;
    }
}
