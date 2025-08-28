using System;
using TMPro;
using UnityEngine;

public class GameCountdownView : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private CanvasGroupShowController showController;
    [SerializeField]
    private TMP_Text countdownText;

    [Header("Timer settings")]
    [Range(1, 10)]
    [SerializeField]
    private int timeUntilResumption = 3;

    private Timer timer = new Timer();
    private Coroutine timerCoroutine;

    private Action OnTimerComplitedAction;

    public void Init()
    {
        showController.ImmediatelyHide();
    }

    public void Dispose()
    {
        KillTimerCoroutine();
    }

    public void StartCountdown(Action OnComplited)
    {
        OnTimerComplitedAction = OnComplited;

        Show();

        KillTimerCoroutine();
        timerCoroutine = StartCoroutine(timer.TimerCoroutine(timeUntilResumption, OnTimerComplited, ChangeCountdownValue));
    }

    public void StopCountdown()
    {
        KillTimerCoroutine();

        Hide();
    }

    private void OnTimerComplited()
    {
        if (OnTimerComplitedAction != null)
        {
            OnTimerComplitedAction?.Invoke();
            OnTimerComplitedAction = null;
        }
        else
        {
            Debug.LogError("On timer complited method not found!!!");
        }

        KillTimerCoroutine();
        Hide();
    }

    private void Show()
    {
        showController.Show();
    }

    private void Hide()
    {
        showController.Hide();
    }

    private void ChangeCountdownValue(int value)
    {
        countdownText.text = value.ToString();
    }

    private void KillTimerCoroutine()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
    }
}