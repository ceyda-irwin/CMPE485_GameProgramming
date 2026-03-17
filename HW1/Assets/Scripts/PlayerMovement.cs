using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveForce = 15f;   // kuvvet
    [SerializeField] float maxSpeed = 6f;     // maksimum hız

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal"); // A-D
        float v = Input.GetAxisRaw("Vertical");   // W-S

        // Kameraya göre yön bul
        Vector3 camForward = Camera.main.transform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = Camera.main.transform.right;
        camRight.y = 0f;
        camRight.Normalize();

        // W/S ileri–geri, A/D sağ–sol
        Vector3 inputDir = (camForward * v + camRight * h).normalized;

        // Kuvvetle hareket
        rb.AddForce(inputDir * moveForce, ForceMode.Acceleration);

        // Hızı yatayda sınırla
        Vector3 vel = rb.velocity;
        Vector3 horiz = new Vector3(vel.x, 0f, vel.z);

        if (horiz.magnitude > maxSpeed)
        {
            horiz = horiz.normalized * maxSpeed;
            rb.velocity = new Vector3(horiz.x, vel.y, horiz.z);
        }
    }
}