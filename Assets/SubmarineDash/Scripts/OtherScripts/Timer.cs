using System;
using System.Collections;
using UnityEngine;

public class Timer
{
    public IEnumerator TimerCoroutine(int seconds, Action OnComplited, Action<int> Countdown = null)
    {
        int secondCounter = seconds;

        while (secondCounter > 0)
        {
            if (Countdown != null)
                Countdown?.Invoke(secondCounter);

            secondCounter--;

            yield return new WaitForSeconds(1f);
        }

        OnComplited?.Invoke();
    }
}