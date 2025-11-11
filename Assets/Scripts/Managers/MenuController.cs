using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject menuButton;

    void Start()
    {
        menuPanel.SetActive(false);
        menuButton.SetActive(true);
    

    }

    public void OpenMenu()
    {
        menuPanel.SetActive(true);
        menuButton.SetActive(false);
        UIBlocker.IsAnyPanelOpen = true;
    }

    public void CloseMenu()
    {
        menuPanel.SetActive(false);
        menuButton.SetActive(true);
        UIBlocker.IsAnyPanelOpen = false;
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
