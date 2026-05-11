using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float verticalSpeed = 6f;
    public float jumpSpeed = 6f;
    public float groundCheckRadius = 0.35f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 2f;
    public Transform cameraTransform;

    [Header("Camera View")]
    public KeyCode toggleViewKey = KeyCode.V;
    public Vector3 thirdPersonLocalOffset = new Vector3(0f, 300f, -100f);
    public float thirdPersonLookAtHeight = 15f;
    public float cameraFollowSpeed = 12f;
    public bool isThirdPerson = false;

    [Header("Movement Limits")]
    public float horizontalLimit = 150f;
    public float minY = 0f;
    public float maxY = 25f;
    public bool isInWaterVolume = true;

    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;
    private Vector3 firstPersonLocalPosition;
    private float cameraPitch = 0f;
    private bool jumpRequested = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        rb.useGravity = false;

        if (cameraTransform != null)
        {
            firstPersonLocalPosition = cameraTransform.localPosition;
        }
    }

    private void Update()
    {
        if (GameManager.Instance != null)
        {
            if (!GameManager.Instance.gameStarted || GameManager.Instance.gameEnded)
            {
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpRequested = true;
        }

        if (Input.GetKeyDown(toggleViewKey))
        {
            isThirdPerson = !isThirdPerson;
        }

        LookAround();
        UpdateCameraView();
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance != null)
        {
            if (!GameManager.Instance.gameStarted || GameManager.Instance.gameEnded)
            {
                rb.velocity = Vector3.zero;
                return;
            }
        }

        MovePlayer();
        UpdateMovementMode();
        ClampVerticalPosition();
    }

    private void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        isInWaterVolume = IsInWaterVolume();

        Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;
        Vector3 finalVelocity = moveDirection.normalized * moveSpeed;

        if (isInWaterVolume)
        {
            float upDown = 0f;

            if (Input.GetKey(KeyCode.Space))
            {
                upDown = 1f;
            }
            else if (Input.GetKey(KeyCode.LeftControl))
            {
                upDown = -1f;
            }

            finalVelocity.y = upDown * verticalSpeed;
        }
        else
        {
            finalVelocity.y = rb.velocity.y;

            if (jumpRequested && IsGrounded())
            {
                finalVelocity.y = jumpSpeed;
            }
        }

        jumpRequested = false;
        rb.velocity = finalVelocity;
    }

    private void UpdateMovementMode()
    {
        rb.useGravity = !isInWaterVolume;
    }

    private bool IsInWaterVolume()
    {
        Vector3 position = rb.position;

        return position.x >= -horizontalLimit &&
               position.x <= horizontalLimit &&
               position.z >= -horizontalLimit &&
               position.z <= horizontalLimit &&
               position.y >= minY &&
               position.y <= maxY;
    }

    private void ClampVerticalPosition()
    {
        Vector3 position = rb.position;
        float clampedY = Mathf.Clamp(position.y, minY, maxY);

        if (!Mathf.Approximately(position.y, clampedY))
        {
            rb.position = new Vector3(position.x, clampedY, position.z);

            Vector3 velocity = rb.velocity;
            if ((position.y > maxY && velocity.y > 0f) ||
                (position.y < minY && velocity.y < 0f))
            {
                velocity.y = 0f;
            }

            rb.velocity = velocity;
        }
    }

    private bool IsGrounded()
    {
        if (capsuleCollider != null && capsuleCollider.bounds.min.y <= minY + 0.15f)
        {
            return true;
        }

        Bounds bounds = capsuleCollider != null ? capsuleCollider.bounds : new Bounds(transform.position, Vector3.one);
        Vector3 origin = new Vector3(bounds.center.x, bounds.min.y + groundCheckRadius + 0.05f, bounds.center.z);
        float radius = Mathf.Min(groundCheckRadius, bounds.extents.x);
        RaycastHit[] hits = Physics.SphereCastAll(
            origin,
            radius,
            Vector3.down,
            0.2f,
            ~0,
            QueryTriggerInteraction.Ignore);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider != null && hits[i].collider.attachedRigidbody != rb)
            {
                return true;
            }
        }

        return false;
    }

    private void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -75f, 75f);
        if (!isThirdPerson && cameraTransform != null)
        {
            cameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
        }
    }

    private void UpdateCameraView()
    {
        if (cameraTransform == null) return;

        if (isThirdPerson)
        {
            Vector3 targetLocalOffset = Quaternion.Euler(cameraPitch, 0f, 0f) * thirdPersonLocalOffset;

            cameraTransform.localPosition = Vector3.Lerp(
                cameraTransform.localPosition,
                targetLocalOffset,
                1f - Mathf.Exp(-cameraFollowSpeed * Time.deltaTime));

            Vector3 lookTarget = transform.TransformPoint(0f, thirdPersonLookAtHeight, 0f);
            cameraTransform.rotation = Quaternion.LookRotation(lookTarget - cameraTransform.position, Vector3.up);
        }
        else
        {
            cameraTransform.localPosition = Vector3.Lerp(
                cameraTransform.localPosition,
                firstPersonLocalPosition,
                1f - Mathf.Exp(-cameraFollowSpeed * Time.deltaTime));

            cameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
        }
    }
}
