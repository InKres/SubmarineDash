using UnityEngine;

public class MainMenuBootstrap : MonoBehaviour
{
    [Header("Models")]
    [SerializeField]
    private ParallaxBackgroundController parallaxBackgroundController;

    [Header("Other components")]
    [SerializeField]
    private ObjectMover objectMover;

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
        parallaxBackgroundController.Init();
        parallaxBackgroundController.EnableBackgroundScrolling();

        objectMover.Init();
    }

    private void Dispose()
    {
        parallaxBackgroundController.Dispose();

        objectMover.Dispose();
    }
}