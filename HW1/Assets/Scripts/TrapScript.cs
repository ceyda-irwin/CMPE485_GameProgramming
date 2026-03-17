using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (gameManager != null)
                gameManager.Lose();
        }
    }
}