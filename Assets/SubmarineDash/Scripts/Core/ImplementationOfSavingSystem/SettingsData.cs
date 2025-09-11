using System;
using UnityEngine;

[Serializable]
public class SettingsData : IPersistentData
{
    public float musicSoundVolume = 0.8f;
    public float efxSoundVolume = 0.8f;

    public string ToJson()
    {
        return JsonUtility.ToJson(this, true);
    }

    public void FromJson(string json)
    {
        JsonUtility.FromJsonOverwrite(json, this);
    }
}