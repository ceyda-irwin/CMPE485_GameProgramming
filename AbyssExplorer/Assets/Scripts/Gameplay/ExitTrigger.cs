using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    public ExitGate gate;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (gate == null) return;

        if (gate.isUnlocked)
        {
            GameManager.Instance.WinGame();
        }
        else
        {
            Debug.Log("Door is locked!");
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowMessage("Collect all runes first!");
            }
        }
    }
} 
