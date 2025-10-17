using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneTransitions : MonoBehaviour
{
    public static SceneTransitions Instance { get; private set; }

    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public static void LoadScene(string CombatScene)
    {
        if (Instance != null)
        {
            Instance.StartCoroutine(Instance.LoadSceneCoroutine(CombatScene));
        }
    }
    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        yield return StartCoroutine(FadeOut());
        SceneManager.LoadScene(sceneName);
        yield return StartCoroutine(FadeIn());
    }

    public IEnumerator FadeOut()
    {
        fadeImage.raycastTarget = true;
        Color c = fadeImage.color;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            c.a = Mathf.Lerp(0, 1, t / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }
        c.a = 1;
        fadeImage.color = c;
    }

    public IEnumerator FadeIn()
    {
        Color c = fadeImage.color;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            c.a = Mathf.Lerp(1, 0, t / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }
        c.a = 0;
        fadeImage.color = c;
        fadeImage.raycastTarget = false;
    }
}
