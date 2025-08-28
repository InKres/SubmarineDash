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

    private ReactiveProperty<int> score = new ReactiveProperty<int>();

    private CompositeDisposable disposables = new CompositeDisposable();

    public void Init(ScoreController scoreController)
    {
        if (scoreController == null)
        {
            Debug.LogError("ScoreController not found!!!");
            return;
        }

        model = scoreController;

        disposables.Add(model.ObserveEveryValueChanged(x => x.Score)
            .Subscribe(scoreValue => score.Value = scoreValue));

        disposables.Add(score.Subscribe(score =>
        {
            scoreText.text = score.ToString();
        }));
    }

    public void Dispose()
    {
        if (model != null)
        {
            disposables.Dispose();

            Time.timeScale = 0;
        }
    }
}