using System;

[Serializable]
public class GameData
{
    public int currentScore;
    public float currentDifficulty;

    public int recordScore;

    public GameData(int currentScore, float currentDifficulty, int recordScore)
    {
        this.currentScore = currentScore;
        this.currentDifficulty = currentDifficulty;

        this.recordScore = recordScore;
    }

    public GameData(GameData gameData)
    {
        currentScore = gameData.currentScore;
        currentDifficulty = gameData.currentDifficulty;

        recordScore = gameData.recordScore;
    }
}