using System;
using UnityEngine;

public class GameSceneUICoordinator : MonoBehaviour
{
    [Header("Presenters")]
    [SerializeField]
    private PauseMenuPresenter pauseMenuPresenter;
    [SerializeField]
    private GameCountdownView countdownView;
    [SerializeField]
    private ScoreControllerPresenter scoreControllerPresenter;
    [SerializeField]
    private GameOverPanelPresenter gameOverPanelPresenter;

    public void InjectPauseMenuPresenter(PauseController pauseController)
    {
        pauseMenuPresenter.Init(pauseController);
    }

    public void InjectScorePresenter(ScoreController scoreController)
    {
        scoreControllerPresenter.Init(scoreController);
    }

    public void InitCountdownView()
    {
        countdownView.Init();
    }

    public void InitGameOverPresenter()
    {
        gameOverPanelPresenter.Init();
    }

    public void Dispose()
    {
        pauseMenuPresenter.Dispose();
        countdownView.Dispose();
        scoreControllerPresenter.Dispose();
        gameOverPanelPresenter.Dispose();
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
}