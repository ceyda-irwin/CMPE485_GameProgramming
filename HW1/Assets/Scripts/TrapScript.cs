using UnityEngine;

public class Trap : MonoBehaviour
{
    [Header("Game Manager")]
    [SerializeField] GameManager gameManager;

    [Header("Movement")]
    [SerializeField] Vector3 upOffset = new Vector3(0f, 1.5f, 0f); // zeminden ne kadar yukarı çıksın
    [SerializeField] float upTime = 0.3f;      // yukarı fırlama süresi
    [SerializeField] float stayUpTime = 0.8f;  // yukarıda kalma süresi
    [SerializeField] float downTime = 0.3f;    // geri inme süresi
    [SerializeField] float stayDownTime = 1.2f;// aşağıda bekleme süresi

    Vector3 downPos;
    Vector3 upPos;
    Collider col;

    void Awake()
    {
        downPos = transform.position;
        upPos = downPos + upOffset;
        col = GetComponent<Collider>();
    }

    void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(TrapRoutine());
    }

    System.Collections.IEnumerator TrapRoutine()
    {
        while (true)
        {
            // Aşağıda bekle (güvenli)
            if (col != null) col.enabled = false;
            transform.position = downPos;
            yield return new WaitForSeconds(stayDownTime);

            // Yukarı fırlama
            float t = 0f;
            if (col != null) col.enabled = true; // artık öldürücü
            while (t < upTime)
            {
                t += Time.deltaTime;
                float k = Mathf.Clamp01(t / upTime);
                transform.position = Vector3.Lerp(downPos, upPos, k);
                yield return null;
            }

            // Yukarıda kal (hala öldürücü)
            transform.position = upPos;
            yield return new WaitForSeconds(stayUpTime);

            // Geri inme
            t = 0f;
            while (t < downTime)
            {
                t += Time.deltaTime;
                float k = Mathf.Clamp01(t / downTime);
                transform.position = Vector3.Lerp(upPos, downPos, k);
                yield return null;
            }

            transform.position = downPos;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (gameManager != null)
                gameManager.Lose();
        }
    }
}