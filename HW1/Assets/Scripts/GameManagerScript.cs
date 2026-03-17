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
        Debug.Log("Win!");
    }

    public void Lose()
    {
        if (hasEnded) return;
        hasEnded = true;

        if (loseUI != null)
            loseUI.SetActive(true);
        Debug.Log("Lose!");
    }

    // UI butonları burayı çağıracak
    public void OnRestartYes()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnRestartNo()
    {
        // İstersen ana menü sahnesi yükleyebilirsin, şimdilik oyundan çık:
        Application.Quit();
        // Editor'de test için:
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}