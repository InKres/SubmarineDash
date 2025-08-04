using System;
using UnityEngine;

public class DifficultyController : MonoBehaviour
{
    public event Action<float> OnChangeDifficultyValue;

    [Header("Settings")]
    [SerializeField]
    [Min(0f)]
    private float startValue = 1f;
    [SerializeField]
    [Min(0f)]
    private float increasePerSecond = 0.01f;

    [Header("READ ONLY")]
    [SerializeField]
    private float m_CurrentDifficultyValue;
    public float CurrentDifficultyValue
    {
        get
        {
            return m_CurrentDifficultyValue;
        }
        private set
        {
            m_CurrentDifficultyValue = value;
        }
    }

    private bool isCanChangingDifficulty;

    private void Update()
    {
        ChangeDifficulty();
    }

    public void Init(float difficultyValue = 0)
    {
        CurrentDifficultyValue = startValue;

        if (difficultyValue > 0)
        {
            CurrentDifficultyValue = difficultyValue;
        }
    }

    public void Dispose()
    {
        DisableChangingDifficulty();
    }

    private void ChangeDifficulty()
    {
        if (isCanChangingDifficulty == false)
            return;

        CurrentDifficultyValue += increasePerSecond * Time.deltaTime;
        OnChangeDifficultyValue?.Invoke(CurrentDifficultyValue);
    }

    public void EnableChangingDifficulty()
    {
        isCanChangingDifficulty = true;
    }

    public void DisableChangingDifficulty()
    {
        isCanChangingDifficulty = false;
    }
}