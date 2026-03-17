using UnityEngine;

public class TrapDistributor : MonoBehaviour
{
    [Header("Template")]
    [SerializeField] Trap trapTemplate;   // Traps altındaki SpikeTrap1

    [Header("Random placement (X/Z)")]
    [SerializeField] Vector2 xRange = new Vector2(20f, 80f);
    [SerializeField] Vector2 zRange = new Vector2(-30f, 30f);
    [SerializeField] int trapCount = 10;

    [ContextMenu("Distribute Traps")]
    void DistributeTraps()
    {
        if (trapTemplate == null)
        {
            Debug.LogError("TrapDistributor: trapTemplate atanmadı.");
            return;
        }

        // Y ve scale şablondan alınır (değişmeyecek)
        float y = trapTemplate.transform.position.y;
        Vector3 scale = trapTemplate.transform.localScale;

        // Eski kopyaları temizle (şablon hariç)
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            var child = transform.GetChild(i);
            if (child.gameObject != trapTemplate.gameObject)
            {
                DestroyImmediate(child.gameObject);
            }
        }

        for (int i = 0; i < trapCount; i++)
        {
            float x = Random.Range(xRange.x, xRange.y);
            float z = Random.Range(zRange.x, zRange.y);
            Vector3 pos = new Vector3(x, y, z);

            Trap newTrap = Instantiate(trapTemplate, pos, trapTemplate.transform.rotation, transform);
            newTrap.transform.localScale = scale;
        }

        Debug.Log($"TrapDistributor: {trapCount} tuzak yerleştirildi.");
    }
}