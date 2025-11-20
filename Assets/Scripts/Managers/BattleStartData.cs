using UnityEngine;

public class BattleStartData : MonoBehaviour
{
    public static string SelectedEnemyID { get; set; } = "";
    public static bool BattleStarting { get; private set; } = false;
    public static bool PlayerFirst { get; set; } = false;

    public static Vector3 LastPlayerPosition { get; set; } = Vector3.zero;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void ResetData()
    {
        BattleStarting = false;
        PlayerFirst = false;
        LastPlayerPosition = Vector3.zero;
    }

    public static bool TryStartBattle(bool playerAttacksFirst)
    {
        if (BattleStarting)
            return false;

        BattleStarting = true;
        PlayerFirst = playerAttacksFirst;
        return true;
    }

    public static void ResetBattleFlag()
    {
        BattleStarting = false;
    }
}
