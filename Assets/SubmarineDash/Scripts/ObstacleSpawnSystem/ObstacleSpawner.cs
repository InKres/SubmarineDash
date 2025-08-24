using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private float spawnDistance = 10f;
    [SerializeField] private float baseSpeed = 5f;
    [SerializeField] private float maxHeightOffset = 1f;

    [Header("Runtime Settings")]
    [SerializeField] private float accelerationFactor = 1f;

    private Camera mainCamera;
    private float cameraWidth;
    private float obstacleWidth;
    private List<GameObject> obstacles = new List<GameObject>();

    public void Init()
    {
        CacheMainCamera();
        CalculateCameraWidth();
        CalculateObstacleWidth();
        SpawnInitialObstacles();
    }

    public void Dispose()
    {
        obstacles.Clear();
    }

    private void Update()
    {
        MoveObstacles();
        HandleSpawning();
        HandleDestruction();
    }

    public void SetAcceleration(float acceleration)
    {
        accelerationFactor = acceleration;
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

    private void CalculateObstacleWidth()
    {
        if (obstaclePrefab == null)
        {
            obstacleWidth = 0f;
            return;
        }

        GameObject sample = Instantiate(obstaclePrefab, Vector3.zero, Quaternion.identity);
        obstacleWidth = sample.GetComponent<Obstacle>().GetObstacleWidth();
        Destroy(sample);
    }

    private void SpawnInitialObstacles()
    {
        float firstSpawnX = cameraWidth / 2f + obstacleWidth / 2f;
        SpawnObstacle(firstSpawnX);

        float secondSpawnX = firstSpawnX + spawnDistance;
        SpawnObstacle(secondSpawnX);
    }

    private void MoveObstacles()
    {
        float moveDistance = baseSpeed * accelerationFactor * Time.deltaTime;

        for (int i = obstacles.Count - 1; i >= 0; i--)
        {
            if (obstacles[i] != null)
            {
                obstacles[i].transform.Translate(Vector3.left * moveDistance);
            }
        }
    }

    private void HandleSpawning()
    {
        if (obstacles.Count == 0) return;

        float rightmostX = GetRightmostObstaclePosition();
        float cameraRightEdge = cameraWidth / 2f;

        if (rightmostX <= cameraRightEdge)
        {
            SpawnObstacle(rightmostX + spawnDistance);
        }
    }

    private float GetRightmostObstaclePosition()
    {
        if (obstacles == null || obstacles.Count == 0)
        {
            return cameraWidth / 2f + obstacleWidth / 2f;
        }

        GameObject lastObstacle = obstacles[obstacles.Count - 1];
        return lastObstacle != null ? lastObstacle.transform.position.x : 0;
    }

    private void HandleDestruction()
    {
        for (int i = obstacles.Count - 1; i >= 0; i--)
        {
            if (obstacles[i] == null)
            {
                obstacles.RemoveAt(i);
                continue;
            }

            float obstacleRightEdge = obstacles[i].transform.position.x + obstacleWidth / 2f;
            float cameraLeftEdge = -cameraWidth / 2f;

            if (obstacleRightEdge < cameraLeftEdge)
            {
                Destroy(obstacles[i]);
                obstacles.RemoveAt(i);
            }
        }
    }

    private void SpawnObstacle(float xPosition)
    {
        float randomYOffset = Random.Range(-maxHeightOffset, maxHeightOffset);
        Vector3 spawnPosition = new Vector3(xPosition, randomYOffset, 0f);

        GameObject newObstacle = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity, transform);
        obstacles.Add(newObstacle);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (mainCamera == null) return;

        Gizmos.color = Color.green;
        float cameraHeight = 2f * mainCamera.orthographicSize;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(cameraWidth, cameraHeight, 0));

        Gizmos.color = Color.red;
        float spawnLineX = cameraWidth / 2f + (obstacleWidth > 0 ? obstacleWidth / 2f : 2f);
        Gizmos.DrawLine(new Vector3(spawnLineX, -5f, 0f), new Vector3(spawnLineX, 5f, 0f));

        Gizmos.color = Color.blue;
        float destroyLineX = -cameraWidth / 2f - (obstacleWidth > 0 ? obstacleWidth / 2f : 2f);
        Gizmos.DrawLine(new Vector3(destroyLineX, -5f, 0f), new Vector3(destroyLineX, 5f, 0f));

        if (maxHeightOffset > 0)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(new Vector3(-cameraWidth / 2f, maxHeightOffset, 0f),
                           new Vector3(cameraWidth / 2f, maxHeightOffset, 0f));
            Gizmos.DrawLine(new Vector3(-cameraWidth / 2f, -maxHeightOffset, 0f),
                           new Vector3(cameraWidth / 2f, -maxHeightOffset, 0f));
        }
    }
#endif
}