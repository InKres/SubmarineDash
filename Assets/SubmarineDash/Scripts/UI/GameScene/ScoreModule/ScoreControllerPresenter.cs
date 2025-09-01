using System;
using UniRx;
using UnityEngine;

public class ScoreControllerPresenter : MonoBehaviour, IDisposable
{
    [Header("View components")]
    [SerializeField]
    private ScoreView scoreView;

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
            scoreView.ChangeScoreValue(score);
        }));
    }

    public void Dispose()
    {
        disposables.Dispose();
    }
}