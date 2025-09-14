using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class SettingsControllerPresenter : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private CanvasGroupShowController showController;

    [Space]
    [SerializeField]
    private Slider musicVolumeSlider;
    [SerializeField]
    private Slider efxVolumeSlider;

    [Space]
    [SerializeField]
    private Button backButton;

    private SettingsController model;

    private CompositeDisposable disposables = new CompositeDisposable();

    public void Init(SettingsController settingsController)
    {
        model = settingsController;

        musicVolumeSlider.onValueChanged.AddListener(OnChangeMusicVolumeSliderValue);
        efxVolumeSlider.onValueChanged.AddListener(OnChangeEFXVolumeSliderValue);
        backButton.onClick.AddListener(OnClickBackButton);

        showController.ImmediatelyHide();

        disposables.Add(model.ObserveEveryValueChanged(model => model.MusicSoundVolume).Subscribe(musicVolume =>
        {
            float volume = musicVolume * 100f;
            if (musicVolumeSlider.value != volume)
            {
                musicVolumeSlider.value = volume;
            }
        }));

        disposables.Add(model.ObserveEveryValueChanged(model => model.EFXSoundVolume).Subscribe(EFXVolume =>
        {
            float volume = EFXVolume * 100f;
            if (efxVolumeSlider.value != volume)
            {
                efxVolumeSlider.value = volume;
            }
        }));
    }

    public void Dispose()
    {
        musicVolumeSlider.onValueChanged.RemoveListener(OnChangeMusicVolumeSliderValue);
        efxVolumeSlider.onValueChanged.RemoveListener(OnChangeEFXVolumeSliderValue);
        backButton.onClick.RemoveListener(OnClickBackButton);

        disposables.Dispose();
    }

    public void Show()
    {
        showController.Show();
    }

    public void Hide()
    {
        showController.Hide();
    }

    private void OnChangeMusicVolumeSliderValue(float value)
    {
        if (model != null)
        {
            model.ChangeMusicSoundVolume((float)Math.Round(value / 100f, 2));
        }
    }

    private void OnChangeEFXVolumeSliderValue(float value)
    {
        if (model != null)
        {
            model.ChangeEFXSoundVolume((float)Math.Round(value / 100f, 2));
        }
    }

    private void OnClickBackButton()
    {
        model.SaveSettings();

        Hide();
    }
}