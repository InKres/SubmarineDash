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

    private float timer = 0f;

    public void Init(int score)
    {
        Score = score;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > 1f)
        {
            Score++;
            timer = 0;
        }
    }
}