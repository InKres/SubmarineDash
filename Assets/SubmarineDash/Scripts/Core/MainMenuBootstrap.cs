using UnityEngine;

public class MainMenuBootstrap : MonoBehaviour
{
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
        parallaxBackgroundController.Init();
        parallaxBackgroundController.EnableBackgroundScrolling();

        playerAnimator.Init();
        playerAnimator.StartAnimation();
    }

    private void Dispose()
    {
        parallaxBackgroundController.Dispose();

        playerAnimator.Dispose();
    }
}