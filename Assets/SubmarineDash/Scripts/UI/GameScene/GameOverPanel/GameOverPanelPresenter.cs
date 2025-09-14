using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanelPresenter : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private CanvasGroupShowController showController;
    [SerializeField]
    private Button replayButton;
    [SerializeField]
    private Button exitToMainMenuButton;

    public void Init()
    {
        showController.ImmediatelyHide();

        replayButton.onClick.AddListener(Replay);
        exitToMainMenuButton.onClick.AddListener(ExitToMainMenu);
    }

    public void Dispose()
    {
        replayButton.onClick.RemoveListener(Replay);
        exitToMainMenuButton.onClick.RemoveListener(ExitToMainMenu);
    }

    public void Show()
    {
        showController.Show();
    }

    public void Replay()
    {
        SceneManager.LoadScene("GameScene"); //TODO: Вынести в модель
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene"); //TODO: Вынести в модель
    }
}