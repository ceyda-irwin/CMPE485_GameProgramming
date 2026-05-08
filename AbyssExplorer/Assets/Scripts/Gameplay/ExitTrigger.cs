using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    public ExitGate gate;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (gate.isUnlocked)
        {
            GameManager.Instance.WinGame();
        }
        else
        {
            Debug.Log("Door is locked!");
        }
    }
}