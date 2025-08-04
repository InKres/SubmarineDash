using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ParallaxBackground : MonoBehaviour
{
    private float baseSpeed;
    private float currentSpeed;
    private float width;
    private float accelerationFactor = 1f;

    private void Awake()
    {
        width = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    public void Init(float baseScrollSpeed)
    {
        baseSpeed = baseScrollSpeed;
        UpdateCurrentSpeed();
    }

    public void SetAccelerationFactor(float newAccelerationFactor)
    {
        accelerationFactor = newAccelerationFactor;
        UpdateCurrentSpeed();
    }

    public void UpdatePosition(float deltaTime)
    {
        transform.Translate(Vector3.left * (currentSpeed * deltaTime));
    }

    public float GetWidth()
    {
        return width;
    }

    public float GetXPosition()
    {
        return transform.position.x;
    }

    public void SetXPosition(float newXPosition)
    {
        transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);
    }

    private void UpdateCurrentSpeed()
    {
        currentSpeed = baseSpeed * accelerationFactor;
    }
}