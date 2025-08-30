using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class SimpleTransformAnimator : MonoBehaviour
{
    [Header("Animation settings")]
    [SerializeField]
    private List<AnimationStep> animationSteps = new List<AnimationStep>();

    [Header("Loop animation settings")]
    [SerializeField]
    private bool isLoop = false;
    [SerializeField]
    private LoopType loopType = LoopType.Restart;

    [Header("Return animation settings")]
    [SerializeField]
    private bool isSaveTransformParametersOnInit = true;
    [SerializeField]
    private Vector3 startPosition;
    [SerializeField]
    private Ease returnMovementEase = Ease.Linear;
    [SerializeField]
    private Vector3 startRotation;
    [SerializeField]
    private Ease returnRotationEase = Ease.Linear;

    [Space]
    [SerializeField]
    private float returnDuration;

    private Sequence movementSequence;
    private Tween movementTween;

    private Sequence rotationSequence;
    private Tween rotationTween;

    public void Init()
    {
        if (isSaveTransformParametersOnInit)
        {
            startPosition = transform.localPosition;
            startRotation = transform.rotation.eulerAngles;
        }
    }

    public void Dispose()
    {
        KillAllTweens();
    }

    public void SetNewAnimationPoints(List<AnimationStep> newAnimationPoints)
    {
        KillAllTweens();

        animationSteps.Clear();
        animationSteps.AddRange(newAnimationPoints);
    }

    public void SetNewLoopType(LoopType loopType)
    {
        this.loopType = loopType;
    }

    [ContextMenu("Start animation")]
    public void StartAnimation()
    {
        StartMovement();
        StartRotatation();
    }

    [ContextMenu("Stop animation")]
    public void StopAnimation()
    {
        StopMovement();
        StopRotatation();
    }

    public void StartMovement()
    {
        KillTween(movementSequence);
        KillTween(movementTween);

        movementSequence = DOTween.Sequence();

        foreach (AnimationStep step in animationSteps)
        {
            movementSequence.Append(transform.DOLocalMove(step.Position, step.Duration)
                .SetEase(step.MovementEase));
        }

        if (isLoop)
            movementSequence.SetLoops(-1, loopType);
    }

    public void StopMovement()
    {
        KillTween(movementSequence);
        KillTween(movementTween);

        movementTween = transform.DOLocalMove(startPosition, returnDuration)
            .SetEase(returnMovementEase);
    }

    public void StartRotatation()
    {
        KillTween(rotationSequence);
        KillTween(rotationTween);

        rotationSequence = DOTween.Sequence();

        foreach (AnimationStep step in animationSteps)
        {
            rotationSequence.Append(transform.DORotate(step.Rotation, step.Duration)
                .SetEase(step.RotationEase).SetOptions(true));
        }

        if (isLoop)
            rotationSequence.SetLoops(-1);
    }

    public void StopRotatation()
    {
        KillTween(rotationSequence);
        KillTween(rotationTween);

        rotationTween = transform.DORotate(startRotation, returnDuration)
            .SetEase(returnRotationEase).SetOptions(true);
    }

    private void KillTween(Tween tween)
    {
        if (tween != null)
        {
            if (tween.IsActive())
            {
                tween.Kill();
            }

            tween = null;
        }
    }

    private void KillAllTweens()
    {
        KillTween(movementSequence);
        KillTween(movementTween);

        KillTween(rotationSequence);
        KillTween(rotationTween);
    }
}