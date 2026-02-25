using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject cratePrefab;
    [SerializeField] private Transform spawnPoint;

    private void Start()
    {
        Debug.Log($"Spawner active. Prefab={(cratePrefab ? cratePrefab.name : "NULL")} SpawnPoint={(spawnPoint ? spawnPoint.name : "NULL")}");
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            Debug.Log("AnyKeyDown detected");
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) Spawn(1);
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) Spawn(2);
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) Spawn(3);
    }

    private void Spawn(int type)
    {
        if (cratePrefab == null)
        {
            Debug.LogError("Spawner: cratePrefab is NULL. Drag your Crate prefab into the slot.");
            return;
        }
        if (spawnPoint == null)
        {
            Debug.LogError("Spawner: spawnPoint is NULL. Drag your SpawnPoint transform into the slot.");
            return;
        }

        GameObject obj = Instantiate(cratePrefab, spawnPoint.position, Quaternion.identity);
        Debug.Log($"Spawned type {type}: {obj.name} at {spawnPoint.position}");

        // type bilgisini ekle
        CrateType ct = obj.GetComponent<CrateType>();
        if (ct == null) ct = obj.AddComponent<CrateType>();
        ct.typeId = type;
    }
}

public class CrateType : MonoBehaviour
{
    public int typeId = 1;
}