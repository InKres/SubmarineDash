using UnityEngine;
using UnityEngine.UI;

public class MainMenuUICoordinator : MonoBehaviour
{
    [Header("Presenters")]
    [SerializeField]
    private ScoreControllerPresenter scoreControllerPresenter;
    [SerializeField]
    private SettingsControllerPresenter settingsControllerPresenter;

    [Header("View components")]
    [SerializeField]
    private Button settingsButton;

    public void Init()
    {
        settingsButton.onClick.AddListener(OpenSettingsPanel);
    }

    public void Dispose()
    {
        settingsButton.onClick.RemoveListener(OpenSettingsPanel);

        scoreControllerPresenter.Dispose();
        settingsControllerPresenter.Dispose();
    }

    public void InitScoreControllerPresenter(ScoreController scoreController)
    {
        scoreControllerPresenter.Init(scoreController);
    }

    public void InitSettingsControllerPresenter(SettingsController settingsController)
    {
        settingsControllerPresenter.Init(settingsController);
    }

    private void OpenSettingsPanel()
    {
        settingsControllerPresenter.Show();
    }
}