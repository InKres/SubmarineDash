using UnityEngine;
using UnityEngine.UI;

public class MainMenuUICoordinator : MonoBehaviour
{
    [Header("private")]
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

    private void OpenSettingsPanel()
    {

    }
}