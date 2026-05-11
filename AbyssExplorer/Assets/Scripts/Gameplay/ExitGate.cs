using UnityEngine;

public class ExitGate : MonoBehaviour
{
    public bool isUnlocked = false;

    [Header("Door Settings")]
    public float openAngle = -90f;
    public float openSpeed = 2f;
    public Collider blockingCollider;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    private bool opening = false;

    private void Start()
    {
        closedRotation = transform.rotation;
        if (blockingCollider == null)
        {
            blockingCollider = GetBlockingCollider();
        }
        if (blockingCollider != null)
        {
            blockingCollider.isTrigger = false;
            blockingCollider.enabled = !isUnlocked;
        }

        openRotation = Quaternion.Euler(
            transform.eulerAngles.x,
            transform.eulerAngles.y + openAngle,
            transform.eulerAngles.z
        );
    }

    private void Update()
    {
        if (opening)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                openRotation,
                openSpeed * Time.deltaTime
            );
        }
    }

    public void UnlockExit()
    {
        if (isUnlocked) return;

        isUnlocked = true;
        opening = true;

        if (blockingCollider != null)
        {
            blockingCollider.enabled = false;
        }

        Debug.Log("Gate opening!");
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
