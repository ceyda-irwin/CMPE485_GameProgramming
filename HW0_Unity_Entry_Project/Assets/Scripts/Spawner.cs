using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject cratePrefab;
    [SerializeField] private Transform spawnPoint; // bunu Player yapacağız
    [SerializeField] private float forwardOffset = 2f;
    [SerializeField] private float upOffset = 1f;

    private void Update()
    {
        if (ScoreManager.Instance != null && ScoreManager.Instance.IsGameOver)
            return;
        if (Input.GetKeyDown(KeyCode.Alpha1)) Spawn(1);
        if (Input.GetKeyDown(KeyCode.Alpha2)) Spawn(2);
        if (Input.GetKeyDown(KeyCode.Alpha3)) Spawn(3);
    }

    private void Spawn(int type)
    {
        if (cratePrefab == null || spawnPoint == null) return;

        Vector3 pos = spawnPoint.position + spawnPoint.forward * forwardOffset + Vector3.up * upOffset;
        GameObject obj = Instantiate(cratePrefab, pos, Quaternion.identity);

        CrateType ct = obj.GetComponent<CrateType>();
        if (ct == null) ct = obj.AddComponent<CrateType>();
        ct.typeId = type;
    }
}

public class CrateType : MonoBehaviour
{
    public int typeId = 1;
}