using UnityEngine;

public class RuneCollectible : MonoBehaviour
{
    [Header("Visual")]
    public float rotateSpeed = 60f;
    public float floatHeight = 0.3f;
    public float floatSpeed = 2f;

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        // Rotation
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);

        // Floating motion
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;

        transform.position = new Vector3(
            transform.position.x,
            newY,
            transform.position.z
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        GameManager.Instance.CollectItem();

        Destroy(gameObject);
    }
}