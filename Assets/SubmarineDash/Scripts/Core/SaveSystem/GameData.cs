using System;

[Serializable]
public class GameData
{
    public int CurrentScore { get; private set; }
    public int CurrentDifficulty { get; private set; }

    public int RecordScore { get; private set; }

    public GameData(int currentScore, int currentDifficulty, int recordScore)
    {
        CurrentScore = currentScore;
        CurrentDifficulty = currentDifficulty;

        RecordScore = recordScore;
    }
}