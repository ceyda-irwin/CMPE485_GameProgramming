using UnityEngine;

public class FishMovement : MonoBehaviour
{
    public float speed = 2f;
    public float moveDistance = 5f;
    public float facingOffsetY = 0f;

    private Vector3 startPos;
    private Vector3 moveAxis;

    void Start()
    {
        startPos = transform.position;
        moveAxis = transform.forward.normalized;
    }

    void Update()
    {
        float movement = Mathf.Sin(Time.time * speed) * moveDistance;
        float direction = Mathf.Cos(Time.time * speed);

        transform.position = startPos + moveAxis * movement;

        // When the fish changes direction at the edges, rotate it accordingly.
        Vector3 targetForward = direction >= 0f ? moveAxis : -moveAxis;
        if (targetForward.sqrMagnitude > 0.0001f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(targetForward, Vector3.up);
            transform.rotation = lookRotation * Quaternion.Euler(0f, facingOffsetY, 0f);
        }
    }
}