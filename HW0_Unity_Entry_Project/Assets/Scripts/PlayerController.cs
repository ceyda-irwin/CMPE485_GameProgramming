using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody rb;

    [Header("Movement")]
    [SerializeField] private float moveForce = 25f;
    [SerializeField] private float maxSpeed = 8f;

    [Header("Homework 2.6: constant force every frame")]
    [SerializeField] private Vector3 extraForce = Vector3.zero;

    private void Reset()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (rb == null) return;

        // 2.6: her frame sabit force
        if (extraForce != Vector3.zero)
            rb.AddForce(extraForce, ForceMode.Force);

        // 2.11: WASD / Arrow input
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, 0f, v).normalized;
        rb.AddForce(dir * moveForce, ForceMode.Force);

        // hız limiti
        Vector3 vel = rb.velocity;
        Vector3 flatVel = new Vector3(vel.x, 0f, vel.z);

        if (flatVel.magnitude > maxSpeed)
        {
            Vector3 limited = flatVel.normalized * maxSpeed;
            rb.velocity = new Vector3(limited.x, vel.y, limited.z);
        }
    }
}