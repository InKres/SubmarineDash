using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuBootstrap : MonoBehaviour
{
    [Header("Bootstrap components")]
    [SerializeField]
    private MainMenuUICoordinator uiCoordinator;

    [Header("Models")]
    [SerializeField]
    private ParallaxBackgroundController parallaxBackgroundController;
    [SerializeField]
    private ScoreController scoreController;
    [SerializeField]
    private SettingsController settingsController;

    [Header("Other components")]
    [SerializeField]
    private SimpleTransformAnimator playerAnimator;

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
    }

    private void OnDestroy()
    {
        Dispose();
    }

    private void LoadGameProgressData()
    {
        gameProgressPersistence.LoadData(data =>
        {
            if (data == null)
            {
                data = new GameProgressData();
            }

            scoreController.Init(data.recordScore);
        });

    }

    private void LoadSettingsData()
    {
        settingsPersistence.LoadData(data =>
        {
            if (data == null)
            {
                Debug.Log("Settings save file not found!");

                data = new SettingsData();
                data.musicSoundVolume = 100f;
                data.efxSoundVolume = 100f;

                settingsPersistence.SaveData(data);
            }

            settingsController.Init(data.musicSoundVolume, data.efxSoundVolume);
        });
    }

    private void Init()
    {
        scoreController.DisableAddingScore();

        parallaxBackgroundController.Init();
        parallaxBackgroundController.EnableBackgroundScrolling();

        playerAnimator.Init();
        playerAnimator.StartAnimation();

        uiCoordinator.Init();
        uiCoordinator.InjectScoreControllerPresenter(scoreController);
        uiCoordinator.InjectSettingsControllerPresenter(settingsController);
        uiCoordinator.OnClickLoadGameScene += OnClickLoadGameScene;
    }

    private void Dispose()
    {
        parallaxBackgroundController.Dispose();

        playerAnimator.Dispose();

        uiCoordinator.OnClickLoadGameScene -= OnClickLoadGameScene;
        uiCoordinator.Dispose();

        settingsController.Dispose();
    }

    private void OnClickLoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }
}