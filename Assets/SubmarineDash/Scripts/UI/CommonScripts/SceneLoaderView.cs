using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoaderView : MonoBehaviour
{
    public event Action OnLoadScene;

    [Header("Components")]
    [SerializeField]
    private Button button;

    [Header("Settings")]
    [SerializeField]
    private string sceneName;

    public void Init()
    {
        button.onClick.AddListener(LoadScene);
    }

    public void Dispose()
    {
        button.onClick.RemoveListener(LoadScene);
    }

    private void LoadScene()
    {
        OnLoadScene?.Invoke();

        SceneManager.LoadScene(sceneName);
    }
}