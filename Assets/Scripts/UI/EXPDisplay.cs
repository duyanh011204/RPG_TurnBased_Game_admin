using UnityEngine;
using TMPro;

public class EXPDisplay : MonoBehaviour
{
    public PlayerStats playerStats;
    public TMP_Text expText;

    void Update()
    {
        if (playerStats != null && expText != null)
        {
            expText.text = $"EXP: {playerStats.exp}/{playerStats.expToNextLevel}";
        }
    }
}
