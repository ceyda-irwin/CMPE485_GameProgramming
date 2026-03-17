using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool hasEnded;

    [Header("UI")]
    public GameObject winUI;
    public GameObject loseUI;

    public void Win()
    {
        if (hasEnded) return;
        hasEnded = true;

        if (winUI != null)
            winUI.SetActive(true);

        // İstersen direkt reset de edebilirsin:
        // Invoke(nameof(Reload), 3f);
    }

    public void Lose()
    {
        if (hasEnded) return;
        hasEnded = true;

        if (loseUI != null)
            loseUI.SetActive(true);

        // Ölümden sonra sahneyi resetle
        // Biraz beklemek istersen:
        Invoke(nameof(Reload), 2f);
    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}