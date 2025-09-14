using System;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    public event Action<bool> OnChangePauseState;

    public bool GameIsPaused { get; private set; }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        GameIsPaused = false;

        OnChangePauseState?.Invoke(GameIsPaused);
    }

    public void Pause()
    {
        GameIsPaused = true;

        OnChangePauseState?.Invoke(GameIsPaused);
    }
}