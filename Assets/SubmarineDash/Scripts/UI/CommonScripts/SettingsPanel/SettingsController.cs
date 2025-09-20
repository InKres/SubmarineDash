using UnityEngine;

public class SettingsController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private AudioController audioController;

    [Space]
    [SerializeField]
    private SettingsPersistence settingsPersistence;

    public float MusicSoundVolume { get; private set; }
    public float EFXSoundVolume { get; private set; }

    public void Init(float musicVolume, float efxVolume)
    {
        MusicSoundVolume = musicVolume;
        EFXSoundVolume = efxVolume;

        audioController.Init();

        ChangeMusicSoundVolume(musicVolume);
        ChangeEFXSoundVolume(efxVolume);
    }

    public void ChangeMusicSoundVolume(float value)
    {
        audioController.ChangeMusicSoundVolume(value);
        MusicSoundVolume = value;
    }

    public void ChangeEFXSoundVolume(float value)
    {
        audioController.ChangeEFXSoundVolume(value);
        EFXSoundVolume = value;
    }

    public void SaveSettings()
    {
        SettingsData data = new SettingsData();
        data.musicSoundVolume = MusicSoundVolume * 100;
        data.efxSoundVolume = EFXSoundVolume * 100;

        settingsPersistence.SaveData(data);
    }
}