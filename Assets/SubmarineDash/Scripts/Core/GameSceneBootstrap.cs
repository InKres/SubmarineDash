using System;
using UnityEngine;

public class GameSceneBootstrap : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private DifficultyController difficultyController;

    private void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        Dispose();
    }

    private void Init()
    {
        difficultyController.Init();
    }

    private void Dispose()
    {
        difficultyController.Dispose();
    }
}