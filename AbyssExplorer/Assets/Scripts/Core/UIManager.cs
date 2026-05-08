using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI")]
    public TMP_Text infoText;

    [Header("References")]
    public OxygenSystem oxygenSystem;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (oxygenSystem == null || GameManager.Instance == null) return;

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

        infoText.text =
            "Oxygen: " + Mathf.CeilToInt(oxygenSystem.currentOxygen) +
            "\nRelics: " + GameManager.Instance.collectedCount + "/" + GameManager.Instance.totalCollectibles;
    }
}