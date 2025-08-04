using System;
using Unity.VisualScripting;
using UnityEngine;

public class GameSceneBootstrap : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private DifficultyController difficultyController;

    [SerializeField]
    private ParallaxBackgroundController backgroundController;

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
        difficultyController.OnChangeDifficultyValue += OnChangeDifficultyValue;

        backgroundController.Init();
    }

    private void Dispose()
    {
        difficultyController.Dispose();
        difficultyController.OnChangeDifficultyValue -= OnChangeDifficultyValue;

        backgroundController.Init();
    }

    private void OnChangeDifficultyValue(float difficultyValue)
    {
        backgroundController.SetAcceleration(difficultyValue);
    }

    private void OnPlay()
    {
        backgroundController.StartScrolling();
    }

    private void OnPause()
    {
        backgroundController.StopScrolling();
    }
}