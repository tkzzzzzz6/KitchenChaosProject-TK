using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : KitchenObjectHolder
{
    public static Player Instance { get; private set; }

    [Header("Movement - Basic")]
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float rotateSpeed = 15f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask counterLayerMask;

    [Header("Movement - Advanced")]
    [Tooltip("Enable smooth acceleration/deceleration")]
    [SerializeField] private bool enableAcceleration = true;
    [SerializeField] private float acceleration = 35f;
    [SerializeField] private float deceleration = 45f;
    [Tooltip("Enable dynamic rotation speed based on angle")]
    [SerializeField] private bool enableDynamicRotation = true;
    [SerializeField] private float minRotateSpeed = 10f;
    [SerializeField] private float maxRotateSpeed = 25f;
    [Tooltip("Enable sliding along walls when blocked")]
    [SerializeField] private bool enableWallSliding = true;

    [Header("Collision")]
    [SerializeField] private float playerRadius = 0.7f;
    [SerializeField] private float playerHeight = 2f;
    [SerializeField] private bool freezeRigidbodyRotationXZ = true;

    [Header("Interaction")]
    [Tooltip("Use wider detection for easier interaction")]
    [SerializeField] private bool useImprovedInteraction = true;
    [SerializeField] private float interactDistance = 2f;
    [SerializeField] private float interactRadius = 0.3f;

    private bool isWalking = false;
    private BaseCounter selectedCounter;
    private Rigidbody rb;
    private Vector3 currentVelocity = Vector3.zero;

    private void Awake()
    {
        Instance = this;

        if (TryGetComponent(out rb) && freezeRigidbodyRotationXZ)
        {
            rb.constraints |= RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnOperateAction += GameInput_OnOperateAction;
    }


    // Update is called once per frame
    private void Update()
    {
        HandleInteraction();
    }
    // usually we put the movement related to physics here

    private void FixedUpdate()
    {
        HandleMovement();
    }

    public bool IsWalking
    {
        get
        {
            return isWalking;
        }
    }

    private void HandleMovement()
    {
        Vector3 desiredDirection = gameInput.GetMovementDirectionNormalized();

        // Calculate velocity with optional acceleration/deceleration
        if (enableAcceleration)
        {
            if (desiredDirection.sqrMagnitude > 0.0001f)
            {
                // Accelerate towards desired direction
                Vector3 targetVelocity = desiredDirection * moveSpeed;
                currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
            }
            else
            {
                // Decelerate to stop
                currentVelocity = Vector3.MoveTowards(currentVelocity, Vector3.zero, deceleration * Time.fixedDeltaTime);
            }

            isWalking = currentVelocity.sqrMagnitude > 0.01f;
        }
        else
        {
            // Original behavior: instant speed
            currentVelocity = desiredDirection * moveSpeed;
            isWalking = desiredDirection.sqrMagnitude > 0.0001f;
        }

        // Calculate move distance and direction
        float moveDistance = currentVelocity.magnitude * Time.fixedDeltaTime;
        Vector3 moveDirection = currentVelocity.normalized;

        // Handle collision and movement
        if (moveDistance > 0.0001f)
        {
            moveDirection = HandleCollision(moveDirection, moveDistance, desiredDirection);
        }
        else
        {
            moveDirection = Vector3.zero;
        }

        // Apply movement
        Vector3 targetPosition = transform.position + moveDirection * moveDistance;
        if (rb != null)
        {
            rb.MovePosition(targetPosition);
        }
        else
        {
            transform.position = targetPosition;
        }

        // Handle rotation with optional dynamic speed
        Vector3 rotationDirection = enableAcceleration ? currentVelocity.normalized : desiredDirection;
        if (rotationDirection.sqrMagnitude > 0.0001f)
        {
            HandleRotation(rotationDirection);
        }
    }

    private Vector3 HandleCollision(Vector3 moveDirection, float moveDistance, Vector3 desiredDirection)
    {
        bool canMove = CanMove(moveDirection, moveDistance);

        if (!canMove)
        {
            // Try wall sliding if enabled
            if (enableWallSliding)
            {
                Vector3 slidingDirection = CalculateWallSliding(moveDirection, moveDistance);
                if (slidingDirection.sqrMagnitude > 0.0001f && CanMove(slidingDirection, moveDistance))
                {
                    return slidingDirection;
                }
            }

            // Fallback to axis separation (original behavior)
            Vector3 moveDirX = new Vector3(desiredDirection.x, 0, 0);
            if (moveDirX.sqrMagnitude > 0.0001f)
            {
                moveDirX = moveDirX.normalized;
                if (CanMove(moveDirX, moveDistance))
                {
                    return moveDirX;
                }
            }

            Vector3 moveDirZ = new Vector3(0, 0, desiredDirection.z);
            if (moveDirZ.sqrMagnitude > 0.0001f)
            {
                moveDirZ = moveDirZ.normalized;
                if (CanMove(moveDirZ, moveDistance))
                {
                    return moveDirZ;
                }
            }

            return Vector3.zero;
        }

        return moveDirection;
    }

    private Vector3 CalculateWallSliding(Vector3 direction, float distance)
    {
        RaycastHit hit;
        Vector3 point1 = transform.position + Vector3.up * playerRadius;
        Vector3 point2 = transform.position + Vector3.up * (playerHeight - playerRadius);

        if (Physics.CapsuleCast(point1, point2, playerRadius, direction, out hit, distance, counterLayerMask))
        {
            // Project movement direction onto the wall surface
            Vector3 normal = hit.normal;
            normal.y = 0; // Keep sliding horizontal
            normal.Normalize();

            Vector3 slideDirection = Vector3.ProjectOnPlane(direction, normal).normalized;
            return slideDirection;
        }

        return Vector3.zero;
    }

    private void HandleRotation(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        float currentRotateSpeed = rotateSpeed;

        if (enableDynamicRotation)
        {
            // Calculate angle difference
            float angleDifference = Quaternion.Angle(transform.rotation, targetRotation);
            // Normalize to 0-1 range
            float normalizedAngle = Mathf.Clamp01(angleDifference / 180f);
            // Use smooth curve: fast for large angles, slow for small angles
            float curveValue = normalizedAngle * normalizedAngle; // Quadratic easing
            currentRotateSpeed = Mathf.Lerp(minRotateSpeed, maxRotateSpeed, curveValue);
        }

        Quaternion newRotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * currentRotateSpeed);

        if (rb != null)
        {
            rb.MoveRotation(newRotation);
        }
        else
        {
            transform.rotation = newRotation;
        }
    }

    private bool CanMove(Vector3 direction, float distance)
    {
        if (direction.sqrMagnitude < 0.0001f)
        {
            return false;
        }

        float radius = Mathf.Max(0.01f, playerRadius);
        float height = Mathf.Max(radius * 2f, playerHeight);

        Vector3 point1 = transform.position + Vector3.up * radius;
        Vector3 point2 = transform.position + Vector3.up * (height - radius);

        bool isBlocked = Physics.CapsuleCast(point1, point2, radius, direction, distance, counterLayerMask);
        return !isBlocked;
    }
    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        selectedCounter?.Interact(this);
    }
    private void GameInput_OnOperateAction(object sender, EventArgs e)
    {
        selectedCounter?.InteractOperate(this);
    }

    private void HandleInteraction()
    {
        RaycastHit hitinfo;
        bool isCollide;

        if (useImprovedInteraction)
        {
            // Use SphereCast for more forgiving interaction detection
            isCollide = Physics.SphereCast(
                transform.position + Vector3.up * 0.5f,
                interactRadius,
                transform.forward,
                out hitinfo,
                interactDistance,
                counterLayerMask
            );
        }
        else
        {
            // Original behavior: precise Raycast
            isCollide = Physics.Raycast(transform.position, transform.forward, out hitinfo, 2f, counterLayerMask);
        }

        if (isCollide)
        {
            if (hitinfo.transform.TryGetComponent<BaseCounter>(out BaseCounter counter))
            {
                SetSelectedCounter(counter);
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    public void SetSelectedCounter(BaseCounter counter)
    {
        if (counter != selectedCounter)
        {
            selectedCounter?.CancelSelect();
            counter?.SelectCounter();
            this.selectedCounter = counter;
        }
    }

}