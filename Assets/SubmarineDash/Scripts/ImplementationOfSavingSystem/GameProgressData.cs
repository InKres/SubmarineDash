using System;
using UnityEngine;

[Serializable]
public class GameProgressData : IPersistentData
{
    public int currentScore;
    public int recordScore;

    public float currentDifficulty;

    public string ToJson()
    {
        return JsonUtility.ToJson(this, true);
    }

    public void FromJson(string json)
    {
        JsonUtility.FromJsonOverwrite(json, this);
    }
}
