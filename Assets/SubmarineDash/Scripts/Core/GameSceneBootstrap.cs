using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField]
    private PauseController pauseController;
    [SerializeField]
    private PauseMenuController pauseMenuController;
    [SerializeField]
    private SettingsController settingsController;

    [Header("Save scripts")]
    [SerializeField]
    private GameProgressPersistence gameProgressPersistence;
    [SerializeField]
    private SettingsPersistence settingsPersistence;

    private void Start()
    {
        LoadGameProgressData();
        LoadSettingsData();

        Init();

        StartGame();
    }

    private void OnDestroy()
    {
        Dispose();
    }

    private void OnApplicationQuit()
    {
        SaveGameProgress();
    }

    private void LoadGameProgressData()
    {
        gameProgressPersistence.LoadData(data =>
        {
            scoreController.Init(data.currentScore);
            Debug.Log($"Current score = {data.currentScore}", this);

            difficultyController.Init(data.currentDifficulty);
            Debug.Log($"Current difficulty = {data.currentDifficulty}", this);
        });
    }

    private void LoadSettingsData()
    {
        settingsPersistence.LoadData(data =>
        {
            settingsController.Init(data.musicSoundVolume, data.efxSoundVolume);
        });

        //Выставить настройки громкости музыки.
        //Выставить настройки громкости эффектов.
    }

    private void Init()
    {
        player.OnGameOver += OnGameOver;

        pauseController.OnChangePauseState += OnChangePauseState;

        uiCoordinator.Init();
        uiCoordinator.InjectPauseMenuPresenter(pauseMenuController);
        uiCoordinator.InjectScorePresenter(scoreController);

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

    private void StartGame()
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

        SaveRecord();

        uiCoordinator.ShowGameOverPanel();
    }

    public void SaveGameProgress() // Его надо как-то вызывать
    {
        GameProgressData data = new GameProgressData();
        data.currentScore = scoreController.Score;
        data.currentDifficulty = difficultyController.CurrentDifficultyValue;

        gameProgressPersistence.SaveData(data);
    }

    private void SaveRecord()
    {
        int currentScore = scoreController.Score;
        int currentRecord = gameProgressPersistence.GetCurrentData().recordScore;

        GameProgressData data = new GameProgressData();
        if (currentRecord < currentScore)
        {
            data.recordScore = currentScore;
        }
        else
        {
            data.recordScore = currentRecord;
        }

        gameProgressPersistence.SaveData(data);
    }

    private void ReloadGameScene()
    {
        SceneManager.LoadScene("GameScene"); // Фу фу фу
    }

    private void LoadMeinMenuScene()
    {
        SceneManager.LoadScene("MainMenuScene"); // Фу фу фу
    }
}