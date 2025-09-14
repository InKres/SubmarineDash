using UnityEngine;
using UnityEngine.UI;

public class MainMenuUICoordinator : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private ScoreView scoreView;
    [SerializeField]
    private Button settingsButton;
    [SerializeField]
    private SceneLoaderView gameSceneLoader;

    public void Init()
    {
        settingsButton.onClick.AddListener(OpenSettingsPanel);
        gameSceneLoader.Init();
    }

    public void Dispose()
    {
        settingsButton.onClick.RemoveListener(OpenSettingsPanel);
        gameSceneLoader.Dispose();
    }

    public void ChangeScore(int score)
    {
        scoreView.ChangeScoreValue(score);
    }

    private void OpenSettingsPanel()
    {

    }
}