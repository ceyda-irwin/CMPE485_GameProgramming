using UnityEngine;

public class TargetZone : MonoBehaviour
{
    [SerializeField] private int acceptedTypeId = 1; // 1/2/3

    private void OnTriggerEnter(Collider other)
    {
        CrateType ct = other.GetComponent<CrateType>();
        if (ct == null) return;

        if (ct.typeId == acceptedTypeId)
        {
            ScoreManager.Instance.AddPoint(1);
            Destroy(other.gameObject); // tekrar saymasın
        }
    }
}