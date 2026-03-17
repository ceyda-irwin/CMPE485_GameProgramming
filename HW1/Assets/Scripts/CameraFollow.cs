using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;   // Player
    [SerializeField] Vector3 offset = new Vector3(0f, 6f, -8f);
    [SerializeField] float followSpeed = 5f;
    [SerializeField] float lookSpeed = 10f;

    void LateUpdate()
    {
        if (target == null) return;

        // Hedef pozisyon (player + offset)
        Vector3 desiredPos = target.position + target.rotation * offset;
        transform.position = Vector3.Lerp(transform.position, desiredPos, followSpeed * Time.deltaTime);

        // Kamerayı player'a doğru döndür
        Quaternion desiredRot = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot, lookSpeed * Time.deltaTime);
    }
}