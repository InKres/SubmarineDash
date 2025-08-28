using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuPresenter : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private CanvasGroupShowController showController;

    [Space]
    [SerializeField]
    private Button resumeButton;
    [SerializeField]
    private Button loadMeinMenuButton;
    [SerializeField]
    private Button exitButton;

    private PauseController model;

    public void Init(PauseController pauseController)
    {
        model = pauseController;
        showController.ImmediatelyHide();

        model.OnChangePauseState += OnChangePauseState;

        resumeButton.onClick.AddListener(Resume);
        loadMeinMenuButton.onClick.AddListener(LoadMainMenu);
        exitButton.onClick.AddListener(Exit);
    }

    public void Dispose()
    {
        model.OnChangePauseState -= OnChangePauseState;

        resumeButton.onClick.RemoveListener(Resume);
        loadMeinMenuButton.onClick.RemoveListener(LoadMainMenu);
        exitButton.onClick.RemoveListener(Exit);
    }

    public void Show()
    {
        showController.Show();
    }

    public void Hide()
    {
        showController.Hide();
    }

    private void OnChangePauseState(bool isPause)
    {
        if (isPause)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Resume()
    {
        //ƒобавить отсчет до того как игра продолжитс€
    }

    private void LoadMainMenu()
    {
        //ƒобавить сохранение прогресса

        SceneManager.LoadScene("MainMenuScene");
    }

    private void Exit()
    {
        //ƒобавить сохранение прогресса

        Application.Quit();
    }
}