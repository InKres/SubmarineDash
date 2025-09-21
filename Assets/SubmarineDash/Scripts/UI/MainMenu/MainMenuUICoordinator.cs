using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUICoordinator : MonoBehaviour
{
    public event Action OnClickLoadGameScene;

    [Header("Presenters")]
    [SerializeField]
    private ScoreControllerPresenter scoreControllerPresenter;
    [SerializeField]
    private SettingsControllerPresenter settingsControllerPresenter;

    [Header("View components")]
    [SerializeField]
    private Button settingsButton;
    [SerializeField]
    private Button playButton;

    public void Init()
    {
        settingsButton.onClick.AddListener(OpenSettingsPanel);
        playButton.onClick.AddListener(OnClickPlayButton);
    }

    public void Dispose()
    {
        settingsButton.onClick.RemoveListener(OpenSettingsPanel);
        playButton.onClick.RemoveListener(OnClickPlayButton);

        scoreControllerPresenter.Dispose();
        settingsControllerPresenter.Dispose();
    }

    public void InjectScoreControllerPresenter(ScoreController scoreController)
    {
        scoreControllerPresenter.Init(scoreController);
    }

    public void InjectSettingsControllerPresenter(SettingsController settingsController)
    {
        settingsControllerPresenter.Init(settingsController);
    }

    private void OpenSettingsPanel()
    {
        settingsControllerPresenter.Show();
    }

    private void OnClickPlayButton()
    {
        OnClickLoadGameScene?.Invoke();
    }
}