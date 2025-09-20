using System;
using TMPro;
using UniRx;
using UnityEngine;

public class ScoreControllerPresenter : MonoBehaviour, IDisposable
{
    [Header("View components")]
    [SerializeField]
    private TMP_Text scoreText;

    private ScoreController model;

    private CompositeDisposable disposables = new CompositeDisposable();

    public void Init(ScoreController scoreController)
    {
        if (scoreController == null)
        {
            Debug.LogError("ScoreController not found!!!");
            return;
        }

        model = scoreController;

        disposables.Add(model.ObserveEveryValueChanged(model => model.Score).Subscribe(scoreValue =>
        {
            scoreText.text = scoreValue.ToString();
        }));
    }

    public void Dispose()
    {
        disposables.Dispose();
    }
}