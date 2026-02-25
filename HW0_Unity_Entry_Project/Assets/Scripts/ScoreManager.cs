using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] private TMP_Text scoreText;
    private int score = 0;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private int winScore = 3;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        UpdateUI();
    }

    public void AddPoint(int amount)
    {
    score += amount;
    UpdateUI();

    if (score >= winScore && winPanel != null)
    {
        winPanel.SetActive(true);
        Time.timeScale = 0f; // oyunu durdurur
    }
    }

    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {score}";
    }
}