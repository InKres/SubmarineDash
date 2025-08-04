using System.Collections.Generic;
using UnityEngine;

//Возможно стоит переписать так, что этот класс координирует все контроллеры слоев, которые знают какой конкретно слой они контролируют, а не все сразу.
public class ParallaxBackgroundController : MonoBehaviour
{
    [System.Serializable]
    public class BackgroundLayerData
    {
        public string name;
        public GameObject prefab;
        public float baseSpeed;
    }

    [SerializeField] private List<BackgroundLayerData> backgroundLayersData = new List<BackgroundLayerData>();
    [SerializeField] private float accelerationFactor = 1f;

    private Camera mainCamera;
    private float cameraWidth;
    private bool isScrolling;
    private Dictionary<string, List<ParallaxBackground>> backgroundLayers = new Dictionary<string, List<ParallaxBackground>>();

    public void Init()
    {
        CacheMainCamera();
        CalculateCameraWidth();
        InitializeAllBackgroundLayers();
    }

    public void Dispose()
    {
        backgroundLayers.Clear();
    }

    private void Update()
    {
        if (IsScrollingDisabled()) return;

        UpdateAllBackgroundLayers();
    }

    public void StartScrolling()
    {
        isScrolling = true;
    }

    public void StopScrolling()
    {
        isScrolling = false;
    }

    public void SetAcceleration(float newAcceleration)
    {
        accelerationFactor = newAcceleration;
        UpdateAllBackgroundsAcceleration();
    }

    private bool IsScrollingDisabled()
    {
        return !isScrolling;
    }

    private void UpdateAllBackgroundLayers()
    {
        float deltaTime = Time.deltaTime;

        foreach (var layer in backgroundLayers.Values)
        {
            UpdateLayerBackgrounds(layer, deltaTime);
            HandleLayerBackgroundPositions(layer);
            UpdateAllBackgroundsAcceleration();
        }
    }

    private void CacheMainCamera()
    {
        mainCamera = Camera.main;
    }

    private void CalculateCameraWidth()
    {
        if (mainCamera == null)
        {
            cameraWidth = 0f;
            return;
        }

        cameraWidth = 2f * mainCamera.orthographicSize * mainCamera.aspect;
    }

    private void InitializeAllBackgroundLayers()
    {
        foreach (BackgroundLayerData layerData in backgroundLayersData)
        {
            List<ParallaxBackground> layerInstances = InitializeBackgroundLayer(layerData);
            backgroundLayers.Add(layerData.name, layerInstances);
        }
    }

    private List<ParallaxBackground> InitializeBackgroundLayer(BackgroundLayerData layerData)
    {
        List<ParallaxBackground> instances = new List<ParallaxBackground>();
        GameObject container = CreateLayerContainer(layerData.name);
        float backgroundWidth = GetBackgroundWidth(layerData.prefab);

        int requiredCount = CalculateRequiredBackgroundCount(backgroundWidth);
        for (int i = 0; i < requiredCount; i++)
        {
            ParallaxBackground bg = CreateBackgroundInstance(layerData, container.transform, backgroundWidth, i);
            instances.Add(bg);
        }

        return instances;
    }

    private ParallaxBackground CreateBackgroundInstance(BackgroundLayerData layerData, Transform parent, float bgWidth, int index)
    {
        Vector3 position = new Vector3(index * (bgWidth - bgWidth * 0.01f), 0, 0);
        GameObject bgObj = Instantiate(layerData.prefab, position, Quaternion.identity, parent);
        bgObj.name = $"{layerData.name}_{index}";

        ParallaxBackground bg = bgObj.GetComponent<ParallaxBackground>();
        bg.Init(layerData.baseSpeed);
        bg.SetAccelerationFactor(accelerationFactor);

        return bg;
    }

    private GameObject CreateLayerContainer(string layerName)
    {
        GameObject container = new GameObject($"Container_{layerName}");
        container.transform.SetParent(transform);
        return container;
    }

    private float GetBackgroundWidth(GameObject prefab)
    {
        GameObject sample = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        float width = sample.GetComponent<ParallaxBackground>().GetWidth();
        Destroy(sample);
        return width;
    }

    private int CalculateRequiredBackgroundCount(float backgroundWidth)
    {
        return Mathf.CeilToInt(cameraWidth / backgroundWidth) + 2;
    }

    private void UpdateAllBackgroundsAcceleration()
    {
        foreach (List<ParallaxBackground> layer in backgroundLayers.Values)
        {
            UpdateLayerAcceleration(layer);
        }
    }

    private void UpdateLayerBackgrounds(List<ParallaxBackground> backgrounds, float deltaTime)
    {
        foreach (ParallaxBackground background in backgrounds)
        {
            background.UpdatePosition(deltaTime);
        }
    }

    private void UpdateLayerAcceleration(List<ParallaxBackground> backgrounds)
    {
        foreach (ParallaxBackground background in backgrounds)
        {
            background.SetAccelerationFactor(accelerationFactor);
        }
    }

    private void HandleLayerBackgroundPositions(List<ParallaxBackground> backgrounds)
    {
        if (backgrounds.Count == 0) return;

        backgrounds.Sort((a, b) => a.GetXPosition().CompareTo(b.GetXPosition()));

        ParallaxBackground firstBg = backgrounds[0];
        ParallaxBackground lastBg = backgrounds[backgrounds.Count - 1];
        float bgWidth = firstBg.GetWidth();

        if (firstBg.GetXPosition() < -bgWidth)
        {
            float newX = lastBg.GetXPosition() + (bgWidth - bgWidth * 0.01f);
            firstBg.SetXPosition(newX);

            backgrounds.RemoveAt(0);
            backgrounds.Add(firstBg);
        }
    }
}