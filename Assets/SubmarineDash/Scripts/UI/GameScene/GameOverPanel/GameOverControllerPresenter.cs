using UnityEngine;
using UnityEngine.UI;

public class GameOverControllerPresenter : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private CanvasGroupShowController showController;
    [SerializeField]
    private Button replayButton;
    [SerializeField]
    private Button exitToMainMenuButton;

    private GameOverController model;

    public void Init(GameOverController gameOverController)
    {
        model = gameOverController;
        model.OnGameOver += Show;

        showController.ImmediatelyHide();

        replayButton.onClick.AddListener(Replay);
        exitToMainMenuButton.onClick.AddListener(ExitToMainMenu);
    }

    public void Dispose()
    {
        if(model != null)
        {
            model.OnGameOver -= Show;
        }

        replayButton.onClick.RemoveListener(Replay);
        exitToMainMenuButton.onClick.RemoveListener(ExitToMainMenu);
    }

    public void Show()
    {
        showController.Show();
    }

    public void Replay()
    {
        if (model == null) return;

        model.Replay();
    }

    public void ExitToMainMenu()
    {
        if (model == null) return;

        model.ExitToMainMenu();
    }
}