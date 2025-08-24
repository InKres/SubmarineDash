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
    [SerializeField]
    private ObstacleSpawner obstacleSpawner;

    private void Start()
    {
        //�������� ��������� ������ ���� ������� � ������ ����� ����� ��������� �������.
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
        obstacleSpawner.Init();

        //TEMP
        difficultyController.EnableChangingDifficulty(); //�������� ���������� �����, ��� ���� ����� �������� ���������� true

        //
    }

    private void Dispose()
    {
        uiCoordinator.Dispose();

        difficultyController.Dispose();
        difficultyController.OnChangeDifficultyValue -= OnChangeDifficultyValue;

        backgroundController.Dispose();

        //TEMP
        difficultyController.DisableChangingDifficulty();
        //
    }

    private void OnChangeDifficultyValue(float difficultyValue)
    {
        backgroundController.SetAcceleration(difficultyValue);
        obstacleSpawner.SetAcceleration(difficultyValue);
    }
}