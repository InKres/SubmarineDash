using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    protected SpriteRenderer[] spriteRenderers;

    public virtual float GetObstacleWidth()
    {
        if (spriteRenderers == null)
        {
            return 0f;
        }

        float maxWidth = 0;
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            if (spriteRenderer.bounds.size.x > maxWidth)
            {
                maxWidth = spriteRenderer.bounds.size.x;
            }
        }

        return maxWidth;
    }
}