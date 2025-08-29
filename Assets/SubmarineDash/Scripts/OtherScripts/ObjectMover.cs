using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class ObjectMover : MonoBehaviour
{
    [Header("Movement settings")]
    [SerializeField]
    private List<Vector3> animationPoints = new List<Vector3>();

    [Header("Animation settings")]
    [SerializeField]
    private float duration = 2f;
    [SerializeField]
    private Ease easeType = Ease.Linear;

    private Vector3 startPosition;

    private Sequence movementSequence;
    private Tween movementTween;

    public void Init()
    {
        startPosition = transform.localPosition;

        StartMovement();
    }

    public void Dispose()
    {
        KillMovementSequence();
        KillMovementTween();
    }

    public void SetNewAnimationPoints(List<Vector3> newAnimationPoints)
    {
        KillMovementSequence();

        animationPoints.Clear();
        animationPoints.AddRange(newAnimationPoints);
    }

    [ContextMenu("Restart")]
    public void StartMovement()
    {
        KillMovementSequence();

        movementSequence = DOTween.Sequence();

        foreach (Vector3 point in animationPoints)
        {
            movementSequence.Append(transform.DOLocalMove(point, duration).SetEase(easeType));
        }

        movementSequence.SetLoops(-1, LoopType.Yoyo);
    }

    public void StopMovement()
    {
        KillMovementSequence();

        movementTween = transform.DOLocalMove(startPosition, duration).SetEase(easeType).OnComplete(() =>
        {
            KillMovementTween();
        });
    }

    private void KillMovementSequence()
    {
        if(movementSequence != null)
        {
            if (movementSequence.IsActive())
            {
                movementSequence.Kill();
            }

            movementSequence = null;
        }
    }

    private void KillMovementTween()
    {
        if (movementTween != null)
        {
            if (movementTween.IsActive())
            {
                movementTween.Kill();
            }

            movementTween = null;
        }
    }
}