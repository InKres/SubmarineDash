using UnityEngine;

public class UICoordinator : MonoBehaviour
{
    [Header("Presenters")]
    [SerializeField]
    private ScoreControllerPresenter scoreModulePresenter;

    public void InjectScorePresenter(ScoreController scoreController)
    {
        scoreModulePresenter.Init(scoreController);
    }

    public void Dispose()
    {
        scoreModulePresenter.Dispose();
    }
}