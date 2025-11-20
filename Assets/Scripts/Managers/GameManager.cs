using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerData playerData;

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
            return;
        }

        if (playerData == null)
            playerData = new PlayerData();
    }

    public void NewGame()
    {
        playerData = new PlayerData();
        LoadScene("GameWorld");
    }

    public void LoadGame()
    {
        // Tạm thời load scene và giữ playerData hiện tại
        LoadScene("GameWorld");
    }

    public void SaveGame()
    {
        // Ví dụ lưu điểm cơ bản bằng PlayerPrefs
        PlayerPrefs.SetInt("attack", playerData.attack);
        PlayerPrefs.SetInt("speed", playerData.speed);
        PlayerPrefs.SetInt("maxHP", playerData.maxHP);
        PlayerPrefs.SetInt("level", playerData.level);
        PlayerPrefs.SetInt("points", playerData.points);
        PlayerPrefs.Save();
        Debug.Log("✅ GameManager: Game saved!");
    }

    public void LoadSavedData()
    {
        if (!PlayerPrefs.HasKey("points")) return;

        playerData.attack = PlayerPrefs.GetInt("attack");
        playerData.speed = PlayerPrefs.GetInt("speed");
        playerData.maxHP = PlayerPrefs.GetInt("maxHP");
        playerData.level = PlayerPrefs.GetInt("level");
        playerData.points = PlayerPrefs.GetInt("points");
        Debug.Log("✅ GameManager: Loaded saved data!");
    }

    public void ResetPlayerData()
    {
        playerData = new PlayerData();
        Debug.Log("✅ GameManager: Player data reset!");
    }

    public void LevelUpPlayer(int pointsToAdd = 5)
    {
        if (playerData.level < 10)
        {
            playerData.level++;
            playerData.points += pointsToAdd;
            Debug.Log($"✅ GameManager: Player leveled up to {playerData.level} with {playerData.points} points!");
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
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
