using UnityEngine;

public class EnterTrigger : MonoBehaviour
{
    public EnterGate gate;

    [Header("Trigger Area")]
    public Vector3 triggerCenter = new Vector3(0f, -10f, 0f);
    public Vector3 triggerSize = new Vector3(24f, 30f, 30f);

    private int playerCount = 0;

    private void Awake()
    {
        if (gate == null)
        {
            gate = GetComponentInParent<EnterGate>();
        }

        BoxCollider triggerCollider = GetComponent<BoxCollider>();
        if (triggerCollider != null)
        {
            triggerCollider.isTrigger = true;
            triggerCollider.center = triggerCenter;
            triggerCollider.size = triggerSize;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsPlayer(other)) return;
        if (gate == null) return;

        playerCount++;
        gate.OpenGate();
        Debug.Log("Enter gate trigger entered.");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsPlayer(other)) return;
        if (gate == null) return;

        playerCount = Mathf.Max(0, playerCount - 1);
        if (playerCount > 0) return;

        gate.CloseGate();
        Debug.Log("Enter gate trigger exited.");
    }

    private bool IsPlayer(Collider other)
    {
        if (other.CompareTag("Player")) return true;
        if (other.GetComponentInParent<PlayerController>() != null) return true;

        Rigidbody attachedRigidbody = other.attachedRigidbody;
        return attachedRigidbody != null && attachedRigidbody.CompareTag("Player");
    }
}
