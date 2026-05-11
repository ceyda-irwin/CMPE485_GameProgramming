using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI")]
    public TMP_Text infoText;

    [Header("Messages")]
    public float defaultMessageDuration = 2f;

    private string currentMessage = "";
    private float messageTimer = 0f;

    [Header("References")]
    public OxygenSystem oxygenSystem;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (oxygenSystem == null || GameManager.Instance == null) return;

        if (messageTimer > 0f)
        {
            messageTimer -= Time.deltaTime;
            if (messageTimer <= 0f)
            {
                currentMessage = "";
            }
        }

        if (GameManager.Instance.gameEnded)
        {
            if (GameManager.Instance.exitUnlocked && GameManager.Instance.collectedCount >= GameManager.Instance.totalCollectibles)
            {
                infoText.text = "Mission Complete!";
            }
            else
            {
                infoText.text = "Oxygen depleted!";
            }

            return;
        }

        string oxygenState = oxygenSystem.isAboveSurface ? "Refilling" : "Underwater";
        string messageLine = string.IsNullOrEmpty(currentMessage) ? "" : currentMessage + "\n";

        infoText.text =
            messageLine +
            "Oxygen: " + Mathf.CeilToInt(oxygenSystem.currentOxygen) +
            "\nRunes: " + GameManager.Instance.collectedCount + "/" + GameManager.Instance.totalCollectibles +
            "\nState: " + oxygenState;

    }

    public void ShowMessage(string message)
    {
        ShowMessage(message, defaultMessageDuration);
    }

    public void ShowMessage(string message, float duration)
    {
        currentMessage = message;
        messageTimer = duration;
    }
}
