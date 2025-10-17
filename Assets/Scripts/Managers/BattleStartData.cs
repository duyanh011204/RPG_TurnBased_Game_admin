using UnityEngine;

public class BattleStartData : MonoBehaviour
{
   
    public static bool BattleStarting { get; private set; } = false;


    public static bool PlayerFirst { get; set; } = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void ResetData()
    {
        BattleStarting = false;
        PlayerFirst = false;
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