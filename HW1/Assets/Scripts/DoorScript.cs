using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Key"))
        {
            // Key kapıya çarptı → kazan
            if (gameManager != null)
                gameManager.Win();
            else
                Debug.Log("Win! (GameManager atanmadı)");
        }
    }
}