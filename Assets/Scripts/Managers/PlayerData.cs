[System.Serializable]
public class PlayerData
{
    public string playerName = "Hero";
    public int level = 1;
    public int currentHP = 100;
    public int maxHP = 100;
    public int currentMP = 50;
    public int maxMP = 50;
    public int attack = 10;
    public int defense = 8;
    public int speed = 12;
    public int experience = 0;
    public int gold = 100;

    // Position
    public float positionX = 0f;
    public float positionY = 0f;
    public string currentScene = "GameWorld";
}