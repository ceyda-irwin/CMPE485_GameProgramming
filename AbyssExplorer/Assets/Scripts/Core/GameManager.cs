using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Collectibles")]
    public int totalCollectibles = 5;
    public int collectedCount = 0;

    [Header("Game State")]
    public bool gameEnded = false;
    public bool exitUnlocked = false;

    [Header("References")]
    public ExitGate exitGate;

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

    public void CollectItem()
    {
        if (gameEnded) return;

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
        Debug.Log("YOU WIN!");
    }

    public void LoseGame()
    {
        if (gameEnded) return;

        gameEnded = true;
        Debug.Log("YOU LOSE!");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}