using System;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public event Action OnClickLoadMainMenuScene;

    public event Action<bool> OnChangePauseState;

    [Header("Components")]
    [SerializeField]
    private PauseController pauseController;

    public bool IsPause => pauseController.IsGameIsPaused;

    public void Init()
    {
        pauseController.OnChangePauseState += (isState) => 
        {
            OnChangePauseState?.Invoke(isState);
        };
    }

    public void Dispose()
    {
        pauseController.OnChangePauseState -= (isState) =>
        {
            OnChangePauseState?.Invoke(isState);
        };
    }

    public void Resume()
    {
        pauseController.Resume();
    }

    public void Pause()
    {
        pauseController.Pause();
    }

    public void LoadMainMenuScene()
    {
        OnClickLoadMainMenuScene?.Invoke();
    }
}