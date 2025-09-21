using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public event Action OnGameOver;

    [Header("Components")]
    [SerializeField]
    private ParticleSystem bubbleParticle;

    [Space]
    [SerializeField]
    private LayerMask obstacleLayer;

    public bool IsDead { get; private set; }

    public void StartParticle()
    {
        bubbleParticle.Play();
    }

    public void StopParticle()
    {
        bubbleParticle.Pause();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsInLayerMask(collision.gameObject.layer, obstacleLayer))
        {
            OnGameOver?.Invoke();

            IsDead = true;
        }
    }

    private bool IsInLayerMask(int layer, LayerMask layerMask)
    {
        return layerMask == (layerMask | (1 << layer));
    }
}