using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SubmarineMovementController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float riseSpeed = 1f;
    [SerializeField] private float diveSpeed = 2f;
    [SerializeField][Range(1f, 20f)] private float transitionSharpness = 8f;

    [Header("Tilt Settings")]
    [SerializeField] private float maxTiltAngle = 15f;
    [SerializeField] private float tiltSpeed = 5f;

    [Header("Input")]
    [SerializeField] private KeyCode diveKey = KeyCode.Space;

    private Rigidbody2D rb;
    private float currentVerticalVelocity;
    private bool isDiving;
    private float targetRotation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        isDiving = Input.GetKey(diveKey);
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleTilt();
    }

    private void HandleMovement()
    {
        float targetVelocity = isDiving ? -diveSpeed : riseSpeed;

        currentVerticalVelocity = Mathf.MoveTowards(
            currentVerticalVelocity,
            targetVelocity,
            transitionSharpness * Time.fixedDeltaTime * Mathf.Abs(currentVerticalVelocity - targetVelocity)
        );

        rb.velocity = new Vector2(rb.velocity.x, currentVerticalVelocity);
    }

    private void HandleTilt()
    {
        targetRotation = Mathf.Clamp(currentVerticalVelocity * maxTiltAngle / diveSpeed, -maxTiltAngle, maxTiltAngle);

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.Euler(0, 0, targetRotation),
            tiltSpeed * Time.fixedDeltaTime
        );
    }
}