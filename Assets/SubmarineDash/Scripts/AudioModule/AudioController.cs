using UniRx;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private AudioSource backgroundSoundSource;
    [SerializeField]
    private AudioSource soundEffectsSource;

    [Header("Settings")]
    [SerializeField]
    private AudioClip backgroundAudioClip;

    private ReactiveProperty<float> globalVolume = new ReactiveProperty<float>();

    private CompositeDisposable disposables = new CompositeDisposable();

    public void Init()
    {
        disposables.Add(globalVolume.Subscribe(volume =>
        {
            backgroundSoundSource.volume = volume;
            soundEffectsSource.volume = volume;
        }));

        backgroundSoundSource.clip = backgroundAudioClip;
        backgroundSoundSource.Play();
    }

    public void Dispose()
    {
        backgroundSoundSource.Stop();
        backgroundSoundSource = null;

        disposables.Dispose();
    }

    public void ChangeGlobalVolume(float volumeValue)
    {
        globalVolume.Value = volumeValue / 100f;
    }

    public void PlayBackgroundSound()
    {
        backgroundSoundSource.Play();
    }

    public void StopBackgroundSound()
    {
        backgroundSoundSource.Stop();
    }

    public void PlaySoundEffect(AudioClip audioClip)
    {
        //if (soundEffectsSource.isPlaying)
        //{
        //    soundEffectsSource.Stop();
        //}

        soundEffectsSource.clip = audioClip;
        soundEffectsSource.Play();
    }
}