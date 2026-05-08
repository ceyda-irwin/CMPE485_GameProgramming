using UnityEngine;

public class OxygenSystem : MonoBehaviour
{
    public float maxOxygen = 100f;
    public float currentOxygen;
    public float oxygenDecreaseRate = 5f;

    private void Start()
    {
        currentOxygen = maxOxygen;
    }

    private void Update()
    {
        if (GameManager.Instance.gameEnded) return;

        currentOxygen -= oxygenDecreaseRate * Time.deltaTime;
        currentOxygen = Mathf.Clamp(currentOxygen, 0f, maxOxygen);

        if (currentOxygen <= 0f)
        {
            GameManager.Instance.LoseGame();
        }
    }
}