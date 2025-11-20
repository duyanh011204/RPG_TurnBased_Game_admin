using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatPanelSystem : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI expText;

    public Button attackPlusButton;
    public Button speedPlusButton;
    public Button healthPlusButton;
    public Button resetButton;

    private PlayerData data;


    void Start()
    {
        data = SaveSystem.LoadData();
        if (data == null)
            data = new PlayerData(); 

        UpdateUI();

        attackPlusButton.onClick.AddListener(() => IncreaseStat("attack"));
        speedPlusButton.onClick.AddListener(() => IncreaseStat("speed"));
        healthPlusButton.onClick.AddListener(() => IncreaseStat("maxHP"));
        resetButton.onClick.AddListener(ResetStats);
    }


    void IncreaseStat(string stat)
    {
        if (data.points <= 0) return;

        switch (stat)
        {
            case "attack": data.attack++; break;
            case "speed": data.speed++; break;
            case "maxHP": data.maxHP++; break;
        }
        data.points--;
        SaveSystem.SaveData(data);
        UpdateUI();
    }

    void ResetStats()
    {
        data = new PlayerData(); // reset tất cả
        SaveSystem.SaveData(data);
        UpdateUI();
    }

    void UpdateUI()
    {
        if (levelText != null) levelText.text = $"STAT LV{data.level}";
        if (pointsText != null) pointsText.text = $"Points: {data.points}";
        if (attackText != null) attackText.text = $"Attack: {data.attack}";
        if (speedText != null) speedText.text = $"Speed: {data.speed}";
        if (healthText != null) healthText.text = $"Health: {data.maxHP}";

        if (expText != null) expText.text = $"EXP: {data.exp}/{data.expToNextLevel}"; // mới

        bool canAdd = data.points > 0;
        attackPlusButton.interactable = canAdd;
        speedPlusButton.interactable = canAdd;
        healthPlusButton.interactable = canAdd;
    }
}
