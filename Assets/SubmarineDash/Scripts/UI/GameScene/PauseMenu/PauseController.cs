using System;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    public event Action<bool> OnChangePauseState;

    public bool IsGameIsPaused { get; private set; }

    public bool isCanPause;

    private void Update()
    {
        if (isCanPause)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (IsGameIsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }
    }

    public void Resume()
    {
        if (IsGameIsPaused == true)
        {
            IsGameIsPaused = false;

            OnChangePauseState?.Invoke(IsGameIsPaused);
        }
    }

    public void Pause()
    {
        if (IsGameIsPaused == false)
        {
            IsGameIsPaused = true;

            OnChangePauseState?.Invoke(IsGameIsPaused);
        }
    }

    public void AllowPausing()
    {
        isCanPause = true;
    }

    public void DisallowPausing()
    {
        isCanPause = false;
    }
}