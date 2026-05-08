using UnityEngine;

public class ExitGate : MonoBehaviour
{
    public bool isUnlocked = false;

    [Header("Materials")]
    public Material lockedMaterial;
    public Material unlockedMaterial;

    private Renderer gateRenderer;

    private void Start()
    {
        gateRenderer = GetComponent<Renderer>();

        if (lockedMaterial != null)
        {
            gateRenderer.material = lockedMaterial;
        }
    }

    public void UnlockExit()
    {
        isUnlocked = true;

        if (unlockedMaterial != null)
        {
            gateRenderer.material = unlockedMaterial;
        }

        Debug.Log("Gate is now open!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (isUnlocked)
        {
            GameManager.Instance.WinGame();
        }
        else
        {
            Debug.Log("Collect all items first!");
        }
    }
}