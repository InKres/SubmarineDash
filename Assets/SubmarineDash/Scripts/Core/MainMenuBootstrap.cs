using UnityEngine;

public class MainMenuBootstrap : MonoBehaviour
{
    [Header("Bootstrap components")]
    [SerializeField]
    private MainMenuUICoordinator uiCoordinator;

    [Header("Models")]
    [SerializeField]
    private ParallaxBackgroundController parallaxBackgroundController;

    [Header("Other components")]
    [SerializeField]
    private SimpleTransformAnimator playerAnimator;

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
        uiCoordinator.Init();

        parallaxBackgroundController.Init();
        parallaxBackgroundController.EnableBackgroundScrolling();

        playerAnimator.Init();
        playerAnimator.StartAnimation();
    }

    private void Dispose()
    {
        uiCoordinator.Dispose();

        parallaxBackgroundController.Dispose();

        playerAnimator.Dispose();
    }
}