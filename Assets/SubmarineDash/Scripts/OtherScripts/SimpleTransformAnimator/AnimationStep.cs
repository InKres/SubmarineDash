using DG.Tweening;
using System;
using UnityEngine;

[Serializable]
public struct AnimationStep
{
    [SerializeField]
    private Vector3 m_Position;
    public Vector3 Position => m_Position;

    [SerializeField]
    private Ease m_MovementEase;
    public Ease MovementEase => m_MovementEase;

    [Space]
    [SerializeField]
    private Vector3 m_Rotation;
    public Vector3 Rotation => m_Rotation;

    [SerializeField]
    private Ease m_RotationEase;
    public Ease RotationEase => m_RotationEase;

    [Space]
    [SerializeField]
    private float m_Duration;
    public float Duration => m_Duration;

    public AnimationStep(Vector3 position, Vector3 rotation, float duration, Ease movementEase = Ease.Linear, Ease rotationEase = Ease.Linear)
    {
        m_Position = position;
        m_MovementEase = movementEase;

        m_Rotation = rotation;
        m_RotationEase = rotationEase;

        m_Duration = duration;
    }
}