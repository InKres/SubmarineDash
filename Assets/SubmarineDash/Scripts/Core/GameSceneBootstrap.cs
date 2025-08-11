using UnityEngine;

public class GameSceneBootstrap : MonoBehaviour
{
    [Header("Bootstrap Components")]
    [SerializeField]
    private UICoordinator uiCoordinator;

    [Header("Models")]
    [SerializeField]
    private DifficultyController difficultyController;
    [SerializeField]
    private ScoreController scoreController;
    [SerializeField]
    private ParallaxBackgroundController backgroundController;

    private void Start()
    {
        //Дописать подгрузку данных всех модулей и только после этого запускать инджект.
        Init();
    }

    private void OnDestroy()
    {
        Dispose();
    }

    private void Init()
    {
        uiCoordinator.InjectScorePresenter(scoreController);

        difficultyController.Init();
        difficultyController.OnChangeDifficultyValue += OnChangeDifficultyValue;

        backgroundController.Init();

        //TEMP
        difficultyController.EnableChangingDifficulty(); //Дописать аккуратный старт, или дать этому значению изначально true
        //
    }

    private void Dispose()
    {
        uiCoordinator.Dispose();

        difficultyController.Dispose();
        difficultyController.OnChangeDifficultyValue -= OnChangeDifficultyValue;

        backgroundController.Init();

        //TEMP
        difficultyController.DisableChangingDifficulty();
        //
    }

    private void OnChangeDifficultyValue(float difficultyValue)
    {
        backgroundController.SetAcceleration(difficultyValue);
    }
}