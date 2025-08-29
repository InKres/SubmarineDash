using UnityEngine;

public class GameSceneBootstrap : MonoBehaviour
{
    [Header("Bootstrap Components")]
    [SerializeField]
    private GameSceneUICoordinator uiCoordinator;

    [Header("Player components")]
    [SerializeField]
    private Player player;
    [SerializeField]
    private SubmarineMovementController submarineMovementController;

    [Header("Models")]
    [SerializeField]
    private DifficultyController difficultyController;
    [SerializeField]
    private ScoreController scoreController;
    [SerializeField]
    private ParallaxBackgroundController backgroundController;
    [SerializeField]
    private ObstacleSpawner obstacleSpawner;

    private PauseController pauseController;

    private void Start()
    {
        //Дописать подгрузку сохранения и только после этого запускать инджект
        Init();

        //Проверить есть ли сохранение, и если нет, то запускать без сохранения
        RunWithoutSaving();
    }

    private void OnDestroy()
    {
        Dispose();
    }

    private void Init()
    {
        player.OnGameOver += OnGameOver;

        pauseController = PauseController.Instance;
        pauseController.OnChangePauseState += OnChangePauseState;

        uiCoordinator.InjectPauseMenuPresenter(pauseController);
        uiCoordinator.InitCountdownView();
        uiCoordinator.InjectScorePresenter(scoreController);
        uiCoordinator.InitGameOverPresenter();

        difficultyController.Init();
        difficultyController.OnChangeDifficultyValue += OnChangeDifficultyValue;

        backgroundController.Init();
        obstacleSpawner.Init();
    }

    private void Dispose()
    {
        player.OnGameOver -= OnGameOver;

        uiCoordinator.Dispose();

        pauseController.OnChangePauseState -= OnChangePauseState;

        difficultyController.Dispose();
        difficultyController.OnChangeDifficultyValue -= OnChangeDifficultyValue;

        backgroundController.Dispose();
        obstacleSpawner.Dispose();
    }

    private void OnChangePauseState(bool isPause)
    {
        if (isPause)
        {
            OnPause();
        }
        else
        {
            uiCoordinator.StartCountdown(OnResume);
        }
    }

    private void OnPause()
    {
        uiCoordinator.StopCountdown();

        player.StopParticle();
        submarineMovementController.DisableAbilityToMove();

        scoreController.DisableAddingScore();
        difficultyController.DisableChangingDifficulty();
        backgroundController.DisableBackgroundScrolling();
        obstacleSpawner.DisableObstaclesScrolling();
    }

    private void OnResume()
    {
        player.StartParticle();
        submarineMovementController.EnableAbilityToMove();

        scoreController.EnableAddingScore();
        difficultyController.EnableChangingDifficulty();
        backgroundController.EnableBackgroundScrolling();
        obstacleSpawner.EnableObstaclesScrolling();
    }

    private void OnChangeDifficultyValue(float difficultyValue)
    {
        backgroundController.SetAcceleration(difficultyValue);

        obstacleSpawner.SetAcceleration(difficultyValue);
    }

    private void RunWithoutSaving()
    {
        scoreController.EnableAddingScore();
        difficultyController.EnableChangingDifficulty();
        backgroundController.EnableBackgroundScrolling();
        obstacleSpawner.EnableObstaclesScrolling();

        player.StartParticle();
        uiCoordinator.StartCountdown(submarineMovementController.EnableAbilityToMove);
    }

    private void OnGameOver()
    {
        OnPause();

        //Дописать отчистку сохранения

        uiCoordinator.ShowGameOverPanel();
    }
}