using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float verticalSpeed = 5f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 2f;
    public Transform cameraTransform;

    private Rigidbody rb;
    private float cameraPitch = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        LookAround();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A-D
        float vertical = Input.GetAxis("Vertical");     // W-S

        Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;

        float upDown = 0f;

        if (Input.GetKey(KeyCode.Space))
        {
            upDown = 1f;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            upDown = -1f;
        }

        Vector3 finalVelocity = moveDirection.normalized * moveSpeed;
        finalVelocity.y = upDown * verticalSpeed;

        rb.velocity = finalVelocity;
    }

    private void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -75f, 75f);

        cameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
    }
}