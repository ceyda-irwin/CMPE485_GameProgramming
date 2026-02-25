using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private int winScore = 3;

    private int score = 0;
    public bool IsGameOver { get; private set; } = false;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        UpdateUI();
        if (winPanel != null) winPanel.SetActive(false);
    }

    private void Update()
    {
        if (IsGameOver && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void AddPoint(int amount)
    {
        if (IsGameOver) return;

        score += amount;
        UpdateUI();

        if (score >= winScore && winPanel != null)
        {
            IsGameOver = true;
            winPanel.SetActive(true);
        }
    }

    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {score}";
    }
}