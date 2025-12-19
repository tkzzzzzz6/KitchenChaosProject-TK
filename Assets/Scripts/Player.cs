using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : KitchenObjectHolder
{
    public static Player Instance { get; private set; }
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float rotateSpeed = 15f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask counterLayerMask;
    [Header("Collision")]
    [SerializeField] private float playerRadius = 0.7f;
    [SerializeField] private float playerHeight = 2f;
    [SerializeField] private bool freezeRigidbodyRotationXZ = true;

    private bool isWalking = false;
    private BaseCounter selectedCounter;
    private Rigidbody rb;

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

        isWalking = desiredDirection.sqrMagnitude > 0.0001f;

        float moveDistance = moveSpeed * Time.fixedDeltaTime;
        Vector3 moveDirection = desiredDirection;

        if (isWalking)
        {
            bool canMove = CanMove(moveDirection, moveDistance);
            if (!canMove)
            {
                Vector3 moveDirX = new Vector3(desiredDirection.x, 0, 0);
                if (moveDirX.sqrMagnitude > 0.0001f)
                {
                    moveDirX = moveDirX.normalized;
                    if (CanMove(moveDirX, moveDistance))
                    {
                        moveDirection = moveDirX;
                        canMove = true;
                    }
                }

                if (!canMove)
                {
                    Vector3 moveDirZ = new Vector3(0, 0, desiredDirection.z);
                    if (moveDirZ.sqrMagnitude > 0.0001f)
                    {
                        moveDirZ = moveDirZ.normalized;
                        if (CanMove(moveDirZ, moveDistance))
                        {
                            moveDirection = moveDirZ;
                            canMove = true;
                        }
                    }
                }

                if (!canMove)
                {
                    moveDirection = Vector3.zero;
                }
            }
        }
        else
        {
            moveDirection = Vector3.zero;
        }

        Vector3 targetPosition = transform.position + moveDirection * moveDistance;
        if (rb != null)
        {
            rb.MovePosition(targetPosition);
        }
        else
        {
            transform.position = targetPosition;
        }

        if (desiredDirection.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(desiredDirection, Vector3.up);
            Quaternion newRotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotateSpeed);

            if (rb != null)
            {
                rb.MoveRotation(newRotation);
            }
            else
            {
                transform.rotation = newRotation;
            }
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

        bool isCollide = Physics.Raycast(transform.position, transform.forward, out hitinfo, 2f, counterLayerMask);

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