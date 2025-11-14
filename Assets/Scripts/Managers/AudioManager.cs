using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Range(0f, 1f)]
    public float masterVolume = 1f;
    public bool isMuted = false;

    private void Awake()
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

        UpdateVolume();
    }

    public void SetVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        UpdateVolume();
    }

    public void ToggleMute(bool mute)
    {
        isMuted = mute;
        UpdateVolume();
    }

    private void UpdateVolume()
    {
        AudioListener.volume = isMuted ? 0f : masterVolume;
    }
}
