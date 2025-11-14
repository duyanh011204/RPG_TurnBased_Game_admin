[System.Serializable]
public class PlayerData
{
    // Stats
    public int attack = 5;
    public int speed = 10;
    public int maxHP = 100;
    public int currentHP = 100;
    public int maxMP = 50;
    public int currentMP = 50;

    // Level & EXP
    public int level = 1;
    public int exp = 0;
    public int expToNextLevel = 100;
    public int points = 5;
    public int pointsPerLevel = 5;
    public int maxLevel = 10;

    // Combat states
    public bool isDead = false;
    public bool isGuarding = false;
    public bool isPoisoned = false;
    public int poisonTurnsRemaining = 0;
}
