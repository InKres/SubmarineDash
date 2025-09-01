using UnityEngine;
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
    private SceneLoaderView mainMenuSceneLoader;

    private PauseController model;

    public void Init(PauseController pauseController)
    {
        model = pauseController;
        showController.ImmediatelyHide();

        model.OnChangePauseState += OnChangePauseState;

        resumeButton.onClick.AddListener(Resume);

        mainMenuSceneLoader.Init();
    }

    public void Dispose()
    {
        model.OnChangePauseState -= OnChangePauseState;

        resumeButton.onClick.RemoveListener(Resume);

        mainMenuSceneLoader.Dispose();
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

    public void Show()
    {
        showController.Show();
    }

    public void Hide()
    {
        showController.Hide();
    }

    public void Resume()
    {
        model.Resume();
    }

    public void Pause()
    {
        model.Pause();
    }
}