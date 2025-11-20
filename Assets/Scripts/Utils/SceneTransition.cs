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

    public static void LoadScene(string sceneName)
    {
        if (Instance != null)
        {
            Instance.StartCoroutine(Instance.LoadSceneCoroutine(sceneName));
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
        if (fadeImage == null)
            yield break;

        fadeImage.raycastTarget = true;
        Color c = fadeImage.color;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            if (fadeImage == null)
                yield break;

            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        c.a = 1f;
        if (fadeImage != null)
            fadeImage.color = c;
    }

    public IEnumerator FadeIn()
    {
        if (fadeImage == null)
            yield break;

        Color c = fadeImage.color;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            if (fadeImage == null)
                yield break;

            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        c.a = 0f;
        if (fadeImage != null)
        {
            fadeImage.color = c;
            fadeImage.raycastTarget = false;
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
