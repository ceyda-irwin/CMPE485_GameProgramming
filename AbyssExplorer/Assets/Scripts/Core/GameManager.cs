using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Collectibles")]
    public int totalCollectibles = 5;
    public int collectedCount = 0;

    [Header("Game State")]
    public bool gameStarted = false;
    public bool gameEnded = false;
    public bool exitUnlocked = false;

    [Header("References")]
    public ExitGate exitGate;
    public GameObject startPanel;
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject gameplayPanel;
    public GameObject performanceCanvas;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Time.timeScale = 0f;

        gameStarted = false;
        gameEnded = false;

        if (startPanel != null) startPanel.SetActive(true);
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
        if (gameplayPanel != null) gameplayPanel.SetActive(false);
        if (performanceCanvas != null) performanceCanvas.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void StartGame()
    {
        gameStarted = true;
        gameEnded = false;

        if (startPanel != null) startPanel.SetActive(false);
        if (gameplayPanel != null) gameplayPanel.SetActive(true);
        if (performanceCanvas != null) performanceCanvas.SetActive(true);

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void CollectItem()
    {
        if (!gameStarted || gameEnded) return;

        collectedCount++;

        Debug.Log("Collected: " + collectedCount + "/" + totalCollectibles);

        if (collectedCount >= totalCollectibles)
        {
            UnlockExit();
        }
    }

    private void UnlockExit()
    {
        exitUnlocked = true;
        Debug.Log("Exit unlocked!");

        if (exitGate != null)
        {
            exitGate.UnlockExit();
        }
    }

    public void WinGame()
    {
        if (gameEnded) return;

        gameEnded = true;

        if (winPanel != null) winPanel.SetActive(true);
        if (gameplayPanel != null) gameplayPanel.SetActive(false);
        if (performanceCanvas != null) performanceCanvas.SetActive(false);

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("YOU WIN!");
    }

    public void LoseGame()
    {
        if (gameEnded) return;

        gameEnded = true;

        if (losePanel != null) losePanel.SetActive(true);
        if (gameplayPanel != null) gameplayPanel.SetActive(false);
        if (performanceCanvas != null) performanceCanvas.SetActive(false);

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("YOU LOSE!");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoHome()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}