using UnityEngine;

public class EnterGate : MonoBehaviour
{
    [Header("Door Settings")]
    public float openAngle = -90f;
    public float openSpeed = 2f;
    public Collider blockingCollider;

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private bool isOpen = false;

    private void Start()
    {
        if (blockingCollider == null)
        {
            blockingCollider = GetBlockingCollider();
        }

        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(
            transform.eulerAngles.x,
            transform.eulerAngles.y + openAngle,
            transform.eulerAngles.z
        );
    }

    private void Update()
    {
        Quaternion targetRotation = isOpen ? openRotation : closedRotation;
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            openSpeed * Time.deltaTime
        );
    }

    public void OpenGate()
    {
        isOpen = true;

        if (blockingCollider != null)
        {
            blockingCollider.enabled = false;
        }
    }

    public void CloseGate()
    {
        isOpen = false;

        if (blockingCollider != null)
        {
            blockingCollider.enabled = true;
        }
    }

    private Collider GetBlockingCollider()
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();

        for (int i = 0; i < colliders.Length; i++)
        {
            if (!colliders[i].isTrigger)
            {
                return colliders[i];
            }
        }

        return null;
    }
}
