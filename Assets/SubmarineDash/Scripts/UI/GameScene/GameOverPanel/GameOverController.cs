using System;
using UnityEngine;

public class GameOverController : MonoBehaviour
{
    public event Action OnGameOver;

    public event Action OnReplay;
    public event Action OnExitToMainMenu;

    [Header("Components")]
    [SerializeField]
    private AudioController audioController;

    [Header("Settings")]
    [SerializeField]
    private AudioClip gameOverSound;

    private Player player;

    public void Init(Player player)
    {
        this.player = player;
        this.player.OnGameOver += GameOver;
    }

    public void Dispose()
    {
        player.OnGameOver -= GameOver;
    }

    private void GameOver()
    {
        OnGameOver?.Invoke();

        audioController.StopBackgroundSound();
        audioController.PlaySoundEffect(gameOverSound);
    }

    public void Replay()
    {
        OnReplay?.Invoke();
    }

    public void ExitToMainMenu()
    {
        OnExitToMainMenu?.Invoke();
    }
}