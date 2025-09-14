using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private int m_Score;
    public int Score
    {
        get
        {
            return m_Score;
        }
        private set
        {
            if (value > 0)
            {
                m_Score = value;
            }
        }
    }

    private bool isCanAddingScore;

    private float timer = 0f;

    public void Init(int score = 0)
    {
        Score = score;
    }

    public void EnableAddingScore()
    {
        isCanAddingScore = true;
    }

    public void DisableAddingScore()
    {
        isCanAddingScore = false;
    }

    private void Update()
    {
        if (isCanAddingScore == false) return;

        timer += Time.deltaTime;

        if (timer > 1f)
        {
            Score++;
            timer = 0;
        }
    }
}