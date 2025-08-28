using System;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    public event Action<bool> OnChangePauseState;

    public static PauseController Instance { get; private set; }

    public bool GameIsPaused { get; private set; }

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance == this)
        {
            Destroy(gameObject);
        }
    }

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