using UnityEngine;

public class AudioController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private AudioSource musicAudioSource;
    [SerializeField]
    private AudioSource efxAudioSource;

    [Header("Settings")]
    [SerializeField]
    private AudioClip backgroundAudioClip;

    public void Init()
    {
        if (backgroundAudioClip != null)
        {
            musicAudioSource.clip = backgroundAudioClip;
            musicAudioSource.Play();
        }
    }

    public void Dispose()
    {
        if (musicAudioSource != null)
        {
            musicAudioSource.Stop();
            musicAudioSource = null;
        }
    }

    public void ChangeMusicSoundVolume(float value)
    {
        musicAudioSource.volume = GetCorrectVolumeValue(value);
    }

    public void ChangeEFXSoundVolume(float value)
    {
        efxAudioSource.volume = GetCorrectVolumeValue(value);
    }

    public void PlayBackgroundSound()
    {
        musicAudioSource.Play();
    }

    public void StopBackgroundSound()
    {
        musicAudioSource.Stop();
    }

    public void PlaySoundEffect(AudioClip audioClip)
    {
        //if (soundEffectsSource.isPlaying)
        //{
        //    soundEffectsSource.Stop();
        //}

        efxAudioSource.clip = audioClip;
        efxAudioSource.Play();
    }

    private float GetCorrectVolumeValue(float value)
    {
        float correctValue = Mathf.Clamp(value, 0, 100f);

        if (correctValue == 0)
        {
            return 0;
        }
        else
        {
            return correctValue / 100f;
        }
    }
}