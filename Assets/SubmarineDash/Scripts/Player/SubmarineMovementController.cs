using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SubmarineMovementController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float riseSpeed = 1f;
    public float diveSpeed = 2f;
    [Range(1f, 20f)]
    public float transitionSharpness = 8f;

    [Header("Tilt Settings")]
    public float maxTiltAngle = 15f;
    public float tiltSpeed = 5f;

    [Header("Input")]
    [SerializeField]
    private KeyCode diveKey = KeyCode.Space;

    private bool isCanMove;

    private Rigidbody2D rb;

    private float currentVerticalVelocity;
    private float targetRotation;
    private float currentRotation;

    private bool isDiving;

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
        if (isCanMove)
        {
            HandleMovement();
            HandleTilt();
        }
    }

    public void EnableAbilityToMove()
    {
        isCanMove = true;
        rb.simulated = true;
    }

    public void DisableAbilityToMove()
    {
        isCanMove = false;

        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.simulated = false;
    }

    private void HandleMovement()
    {
        float targetVelocity = isDiving ? -diveSpeed : riseSpeed;

        float maxDelta = transitionSharpness * Time.fixedDeltaTime *
            Mathf.Abs(currentVerticalVelocity - targetVelocity);

        currentVerticalVelocity = Mathf.MoveTowards(
            currentVerticalVelocity,
            targetVelocity,
            maxDelta
        );

        Vector2 newPosition = rb.position + new Vector2(0f, currentVerticalVelocity) * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }

    private void HandleTilt()
    {
        targetRotation = Mathf.Clamp(currentVerticalVelocity * maxTiltAngle / diveSpeed, -maxTiltAngle, maxTiltAngle);

        currentRotation = Mathf.Lerp(
            currentRotation,
            targetRotation,
            tiltSpeed * Time.fixedDeltaTime
        );

        rb.MoveRotation(currentRotation);
    }
}