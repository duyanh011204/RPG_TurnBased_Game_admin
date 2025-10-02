using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
        
    public void NewGame()
    {
        LoadScene("GameWorld");
    }

    public void LoadGame()
    {
        // TODO: Implement load functionality
        LoadScene("GameWorld");
    }

    public void StartCombat()
    {
        LoadScene("Combat");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}