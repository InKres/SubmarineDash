using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuControllerPresenter : MonoBehaviour
{
    public event Action<bool> OnChangeShowState;
    public event Action OnShowSettingsPanel;

    [Header("Components")]
    [SerializeField]
    private CanvasGroupShowController showController;

    [Space]
    [SerializeField]
    private Button resumeButton;
    [SerializeField]
    private Button settingsButton;
    [SerializeField]
    private Button loadMainMenuButton;

    private PauseMenuController model;

    public void Init(PauseMenuController pauseController)
    {
        model = pauseController;
        showController.ImmediatelyHide();

        model.OnChangePauseState += OnChangePauseState;

        resumeButton.onClick.AddListener(Resume);
        settingsButton.onClick.AddListener(OnClickSettingsButton);
        loadMainMenuButton.onClick.AddListener(OnClickLoadMainMenuSceneButton);
    }

    public void Dispose()
    {
        model.OnChangePauseState -= OnChangePauseState;

        resumeButton.onClick.RemoveListener(Resume);
        settingsButton.onClick.RemoveListener(OnClickSettingsButton);
        loadMainMenuButton.onClick.RemoveListener(OnClickLoadMainMenuSceneButton);
    }

    public void Show()
    {
        if (model == null) return;

        showController.Show();
        model.Pause();

        OnChangeShowState?.Invoke(true);
    }

    public void Hide()
    {
        if (model == null) return;

        showController.Hide();
        model.Resume();

        OnChangeShowState?.Invoke(false);
    }

    private void Resume()
    {
        if (model == null) return;

        model.Resume();
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

    private void OnClickSettingsButton()
    {
        OnShowSettingsPanel?.Invoke();
    }

    private void OnClickLoadMainMenuSceneButton()
    {
        if (model == null) return;

        model.LoadMainMenuScene();
    }
}
