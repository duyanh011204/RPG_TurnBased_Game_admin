using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [Header("Menu Panel")]
    public GameObject menuPanel;
    public GameObject menuButton;

    [Header("Settings Panel")]
    public GameObject settingsPanel;
    public Slider volumeSlider;
    public Toggle muteToggle;

    void Start()
    {
        menuPanel.SetActive(false);
        menuButton.SetActive(true);

        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        if (volumeSlider != null)
            volumeSlider.value = AudioManager.Instance.masterVolume;

        if (muteToggle != null)
            muteToggle.isOn = AudioManager.Instance.isMuted;

        if (volumeSlider != null)
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);

        if (muteToggle != null)
            muteToggle.onValueChanged.AddListener(OnMuteChanged);
    }

    public void OpenMenu()
    {
        menuPanel.SetActive(true);
        menuButton.SetActive(false);
       
    }

    public void CloseMenu()
    {
        menuPanel.SetActive(false);
        menuButton.SetActive(true);
        
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenSettings()
    {
        if (settingsPanel == null) return;

        settingsPanel.SetActive(true);
        menuPanel.SetActive(false);

        if (muteToggle != null)
            muteToggle.isOn = !AudioManager.Instance.isMuted;
    }

    public void CloseSettings()
    {
        if (settingsPanel == null) return;

        settingsPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    private void OnVolumeChanged(float value)
    {
        AudioManager.Instance.SetVolume(value);
    }

    private void OnMuteChanged(bool isOn)
    {
       
        AudioManager.Instance.ToggleMute(!isOn);
    }
}
