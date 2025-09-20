using System;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneUICoordinator : MonoBehaviour
{
    [Header("Presenters")]
    [SerializeField]
    private PauseMenuControllerPresenter pauseMenuPresenter;
    [SerializeField]
    private SettingsControllerPresenter settingsControllerPresenter;
    [SerializeField]
    private ScoreControllerPresenter scoreControllerPresenter;
    [SerializeField]
    private GameOverPanelPresenter gameOverPanelPresenter;

    [Header("View")]
    [SerializeField]
    private Button pauseButton;
    [SerializeField]
    private GameCountdownView countdownView;

    public void Init()
    {
        pauseButton.onClick.AddListener(OnClickPauseButton);

        InitCountdownView();
        InitGameOverPresenter();
    }

    public void Dispose()
    {
        pauseButton.onClick.RemoveListener(OnClickPauseButton);

        pauseMenuPresenter.Dispose();
        settingsControllerPresenter.Dispose();
        scoreControllerPresenter.Dispose();
        gameOverPanelPresenter.Dispose();

        countdownView.Dispose();
    }

    public void InjectPauseMenuPresenter(PauseMenuController pauseController)
    {
        pauseMenuPresenter.Init(pauseController);
    }

    public void InjectScorePresenter(ScoreController scoreController)
    {
        scoreControllerPresenter.Init(scoreController);
    }

    public void InitSettingsControllerPresenter(SettingsController settingsController)
    {
        settingsControllerPresenter.Init(settingsController);
    }

    private void InitCountdownView()
    {
        countdownView.Init();
    }

    private void InitGameOverPresenter()
    {
        gameOverPanelPresenter.Init();
    }

    public void StartCountdown(Action OnTimerComplited)
    {
        countdownView.StartCountdown(OnTimerComplited);
    }

    public void StopCountdown()
    {
        countdownView.StopCountdown();
    }

    public void ShowGameOverPanel()
    {
        gameOverPanelPresenter.Show();
    }

    private void OnClickPauseButton()
    {
        pauseMenuPresenter.Show();
    }
}