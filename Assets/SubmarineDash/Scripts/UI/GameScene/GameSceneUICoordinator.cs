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
    private GameOverControllerPresenter gameOverControllerPresenter;

    [Header("View")]
    [SerializeField]
    private Button pauseButton;
    [SerializeField]
    private GameCountdownView countdownView;

    public void Init()
    {
        pauseButton.onClick.AddListener(OnClickPauseButton);

        InitCountdownView();
    }

    public void Dispose()
    {
        pauseButton.onClick.RemoveListener(OnClickPauseButton);

        pauseMenuPresenter.Dispose();
        pauseMenuPresenter.OnChangeShowState -= OnChangePauseMenuShowState;
        pauseMenuPresenter.OnShowSettingsPanel -= ShowSettingsPanel;

        settingsControllerPresenter.Dispose();
        scoreControllerPresenter.Dispose();
        gameOverControllerPresenter.Dispose();

        countdownView.Dispose();
    }

    public void InjectPauseMenuPresenter(PauseMenuController pauseController)
    {
        pauseMenuPresenter.Init(pauseController);
        pauseMenuPresenter.OnChangeShowState += OnChangePauseMenuShowState;
        pauseMenuPresenter.OnShowSettingsPanel += ShowSettingsPanel;
    }

    public void InjectScorePresenter(ScoreController scoreController)
    {
        scoreControllerPresenter.Init(scoreController);
    }

    public void InjectSettingsControllerPresenter(SettingsController settingsController)
    {
        settingsControllerPresenter.Init(settingsController);
    }

    public void InjectGameOverPresenter(GameOverController gameOverController)
    {
        gameOverControllerPresenter.Init(gameOverController);
    }

    public void StartCountdown(Action OnTimerComplited)
    {
        countdownView.StartCountdown(OnTimerComplited);
    }

    public void StopCountdown()
    {
        countdownView.StopCountdown();
    }

    private void InitCountdownView()
    {
        countdownView.Init();
    }

    private void OnClickPauseButton()
    {
        pauseMenuPresenter.Show();
    }

    private void OnChangePauseMenuShowState(bool isState)
    {
        if (isState == false)
        {
            if (settingsControllerPresenter.IsShown)
            {
                settingsControllerPresenter.ClosePanel();
            }
        }
    }

    private void ShowSettingsPanel()
    {
        settingsControllerPresenter.Show();
    }
}