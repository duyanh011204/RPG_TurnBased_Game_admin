using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatHealthUI : MonoBehaviour
{
    [Header("Player UI")]
    public Slider playerHPBar;
    public TextMeshProUGUI playerHPText;
    public TextMeshProUGUI playerNameText;

    [Header("Enemy UI")]
    public Slider enemyHPBar;
    public TextMeshProUGUI enemyHPText;
    public TextMeshProUGUI enemyNameText;

    public void UpdatePlayerHP(int current, int max)
    {
        playerHPBar.value = (float)current / max;
        playerHPText.text = $"{current} / {max}";
    }

    public void UpdateEnemyHP(int current, int max)
    {
        enemyHPBar.value = (float)current / max;
        enemyHPText.text = $"{current} / {max}";
    }

    public void SetPlayerName(string name)
    {
        playerNameText.text = name;
    }

    public void SetEnemyName(string name)
    {
        enemyNameText.text = name;
    }
}