using UnityEngine;

public class Collectible : MonoBehaviour
{
    public float rotateSpeed = 80f;

    void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.CollectItem();
            Destroy(gameObject);
        }
    }
}