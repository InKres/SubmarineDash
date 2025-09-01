using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private TMP_Text scoreText;

    public void ChangeScoreValue(int value)
    {
        scoreText.text = value.ToString();
    }
}