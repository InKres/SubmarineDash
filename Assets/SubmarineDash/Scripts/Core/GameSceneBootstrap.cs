using System;
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
    [SerializeField]
    private GameOverController gameOverController;

    [Header("Save scripts")]
    [SerializeField]
    private GameProgressPersistence gameProgressPersistence;
    [SerializeField]
    private SettingsPersistence settingsPersistence;

    private void Start()
    {
        gameProgressPersistence.Init();
        settingsPersistence.Init();
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
        if (player.IsDead == false)
        {
            SaveGameProgress();
        }
        else
        {
            SaveRecord(); //На случай, если игрок психанет, когда програет и решит закрыть игру
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus == false)
        {
            if (player.IsDead == false)
            {
                SaveGameProgress(); // На всякий случай
            }
            else
            {
                SaveRecord(); //На случай, если игрок психанет, когда програет
            }
        }
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
    }

    private void Init()
    {
        gameOverController.OnGameOver += OnGameOver;

        pauseController.AllowPausing();
        pauseController.OnChangePauseState += OnChangePauseState;
        pauseMenuController.Init();
        pauseMenuController.OnClickLoadMainMenuScene += () => 
        {
            if (player.IsDead == false)
            {
                SaveGameProgress(LoadMainMenuScene);
            }
        };

        uiCoordinator.Init();
        uiCoordinator.InjectPauseMenuPresenter(pauseMenuController);
        uiCoordinator.InjectScorePresenter(scoreController);
        uiCoordinator.InjectGameOverPresenter(gameOverController);
        uiCoordinator.InjectSettingsControllerPresenter(settingsController);

        difficultyController.OnChangeDifficultyValue += OnChangeDifficultyValue;

        gameOverController.Init(player);
        gameOverController.OnReplay += ReloadGameScene;
        gameOverController.OnExitToMainMenu += LoadMainMenuScene;

        backgroundController.Init();
        obstacleSpawner.Init();
    }

    private void Dispose()
    {
        player.OnGameOver -= OnGameOver;

        uiCoordinator.Dispose();

        pauseController.OnChangePauseState -= OnChangePauseState;
        pauseMenuController.OnClickLoadMainMenuScene -= () =>
        {
            if (player.IsDead == false)
            {
                SaveGameProgress(LoadMainMenuScene);
            }
        };
        pauseMenuController.Dispose();

        gameOverController.Dispose();
        gameOverController.OnReplay -= ReloadGameScene;
        gameOverController.OnExitToMainMenu -= LoadMainMenuScene;

        difficultyController.Dispose();
        difficultyController.OnChangeDifficultyValue -= OnChangeDifficultyValue;

        settingsController.Dispose();

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
        pauseController.DisallowPausing();
        OnPause();

        SaveRecord();
    }

    public void SaveGameProgress(Action onSuccess = null)
    {
        GameProgressData data = new GameProgressData();
        data.currentScore = scoreController.Score;
        data.currentDifficulty = difficultyController.CurrentDifficultyValue;
        data.recordScore = gameProgressPersistence.GetCurrentData().recordScore;

        gameProgressPersistence.SaveData(data, onSuccess);
    }

    private void SaveRecord(Action onSuccess = null)
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

        gameProgressPersistence.SaveData(data, onSuccess);
    }

    private void ReloadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void LoadMainMenuScene()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}