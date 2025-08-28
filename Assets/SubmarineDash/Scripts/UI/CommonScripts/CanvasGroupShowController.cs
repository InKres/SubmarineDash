using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupShowController : MonoBehaviour
{
    public event Action OnShowed = delegate { };
    public event Action OnHided = delegate { };

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    public float speed = 4f;

    public bool IsShown
    {
        get;
        private set;
    }

    private void Reset()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnDisable()
    {
        if (IsShown)
        {
            ImmediatelyShow();
        }
        else
        {
            ImmediatelyHide();
        }
    }

    [ContextMenu("Show")]
    public void Show()
    {
        StopAllCoroutines();
        if (this.isActiveAndEnabled)
            StartCoroutine(ShowCoroutine());
        else
            ImmediatelyShow();
    }

    public void Show(float speed)
    {
        IsShown = true;
        StopAllCoroutines();
        StartCoroutine(ShowCoroutine(speed));
    }

    [ContextMenu("Hide")]
    public void Hide()
    {
        IsShown = false;
        StopAllCoroutines();
        if (this.isActiveAndEnabled)
        {
            StartCoroutine(HideCoroutine());
        }
        else
        {
            ImmediatelyHide();
        }
    }

    public void Hide(float speed)
    {
        StopAllCoroutines();
        StartCoroutine(HideCoroutine(speed));
    }

    public void ImmediatelyShow()
    {
        StopAllCoroutines();
        IsShown = true;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1;
        OnShowed.Invoke();
    }

    public void ImmediatelyHide()
    {
        StopAllCoroutines();
        IsShown = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;
        OnHided.Invoke();
    }

    private IEnumerator ShowCoroutine()
    {
        IsShown = true;
        canvasGroup.blocksRaycasts = true;
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime * speed;
            yield return new WaitForEndOfFrame();
        }

        OnShowed.Invoke();
    }

    private IEnumerator ShowCoroutine(float speed)
    {
        IsShown = true;
        canvasGroup.blocksRaycasts = true;
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime * speed;
            yield return new WaitForEndOfFrame();
        }

        OnShowed.Invoke();
    }

    private IEnumerator HideCoroutine()
    {
        IsShown = false;
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * speed;
            yield return new WaitForEndOfFrame();
        }
        canvasGroup.blocksRaycasts = false;

        OnHided.Invoke();
    }

    private IEnumerator HideCoroutine(float speed)
    {
        IsShown = false;
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * speed;
            yield return new WaitForEndOfFrame();
        }
        canvasGroup.blocksRaycasts = false;

        OnHided.Invoke();
    }
}