using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Guard : MonoBehaviour
{
    [Header("Game Manager")]
    [SerializeField] GameManager gameManager;

    [Header("Patrol")]
    [SerializeField] Vector3 localOffset = new Vector3(0f, 0f, 10f); // ne kadar ileri gitsin
    [SerializeField] float moveTime = 2f;     // ileri / geri süresi
    [SerializeField] float waitTime = 0.5f;   // uçlarda bekleme
    [SerializeField] float moveSpeed = 4f;    // rigidbody hız limiti

    Rigidbody rb;
    Vector3 pointA;
    Vector3 pointB;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        pointA = transform.position;
        pointB = transform.position + localOffset;
    }

    void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(PatrolRoutine());
    }

    System.Collections.IEnumerator PatrolRoutine()
    {
        while (true)
        {
            yield return MoveBetween(pointA, pointB);
            yield return new WaitForSeconds(waitTime);
            yield return MoveBetween(pointB, pointA);
            yield return new WaitForSeconds(waitTime);
        }
    }

    System.Collections.IEnumerator MoveBetween(Vector3 from, Vector3 to)
    {
        float t = 0f;
        while (t < moveTime)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / moveTime);
            Vector3 targetPos = Vector3.Lerp(from, to, k);

            // rb ile hareket
            Vector3 dir = (targetPos - transform.position);
            rb.velocity = dir.normalized * moveSpeed;

            // yüzünü gittiği yöne çevir
            if (dir.sqrMagnitude > 0.0001f)
            {
                Quaternion look = Quaternion.LookRotation(new Vector3(dir.x, 0f, dir.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, look, 10f * Time.deltaTime);
            }

            yield return null;
        }

        // noktaya oturt
        transform.position = to;
        rb.velocity = Vector3.zero;
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