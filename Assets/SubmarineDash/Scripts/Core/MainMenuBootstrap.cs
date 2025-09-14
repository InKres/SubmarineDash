using UnityEngine;

public class MainMenuBootstrap : MonoBehaviour
{
    [Header("Bootstrap components")]
    [SerializeField]
    private MainMenuUICoordinator uiCoordinator;

    [Header("Models")]
    [SerializeField]
    private ParallaxBackgroundController parallaxBackgroundController;

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
            uiCoordinator.ChangeScore(data.recordScore);
        });

    }

    private void LoadSettingsData()
    {
        settingsPersistence.LoadData(data =>
        {

        });

        //Выставить настройки громкости музыки.
        //Выставить настройки громкости эффектов.
    }

    private void Init()
    {
        uiCoordinator.Init();

        parallaxBackgroundController.Init();
        parallaxBackgroundController.EnableBackgroundScrolling();

        playerAnimator.Init();
        playerAnimator.StartAnimation();
    }

    private void Dispose()
    {
        uiCoordinator.Dispose();

        parallaxBackgroundController.Dispose();

        playerAnimator.Dispose();
    }
}