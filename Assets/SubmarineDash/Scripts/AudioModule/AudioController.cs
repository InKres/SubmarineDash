using System;
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
        musicAudioSource.clip = backgroundAudioClip;
        musicAudioSource.Play();
    }

    public void Dispose()
    {
        musicAudioSource.Stop();
        musicAudioSource = null;
    }

    public void ChangeMusicSoundVolume(float value)
    {
        musicAudioSource.volume = Math.Clamp(value, 0f, 1f);
    }

    public void ChangeEFXSoundVolume(float value)
    {
        efxAudioSource.volume = Math.Clamp(value, 0f, 1f);
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
}