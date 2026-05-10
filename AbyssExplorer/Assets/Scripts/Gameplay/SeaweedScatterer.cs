using System.Collections.Generic;
using UnityEngine;

public class SeaweedScatterer : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] seaweedPrefabs;

    [Header("Spawn Area (centered on this object)")]
    public float halfSizeX = 150f;
    public float halfSizeZ = 150f;
    public int spawnCount = 80;
    public float minSpacing = 8f;
    
    [Header("Scale")]
    public float minScale = 0.8f;
    public float maxScale = 1.4f;

    [Header("Height")]
    public bool raycastToGround = true;
    public LayerMask groundMask = ~0;
    public float raycastStartY = 200f;
    public float raycastDistance = 600f;
    public float fallbackY = 0f;

    [Header("Generation")]
    public bool spawnOnStart = false;
    public int maxPlacementAttempts = 5000;

    private readonly List<Vector3> placedPositions = new List<Vector3>();

    private void Start()
    {
        if (spawnOnStart)
        {
            GenerateSeaweed();
        }
    }

    [ContextMenu("Generate Seaweed")]
    public void GenerateSeaweed()
    {
        ClearChildren();
        placedPositions.Clear();

        if (seaweedPrefabs == null || seaweedPrefabs.Length == 0)
        {
            Debug.LogWarning("SeaweedScatterer: No prefabs assigned.");
            return;
        }

        int spawned = 0;
        int attempts = 0;

        while (spawned < spawnCount && attempts < maxPlacementAttempts)
        {
            attempts++;

            Vector3 candidate = GetRandomPositionXZ();
            if (!IsFarEnough(candidate))
            {
                continue;
            }

            if (raycastToGround)
            {
                Vector3 rayOrigin = new Vector3(candidate.x, transform.position.y + raycastStartY, candidate.z);
                if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, raycastDistance, groundMask, QueryTriggerInteraction.Ignore))
                {
                    candidate.y = hit.point.y;
                }
                else
                {
                    continue;
                }
            }
            else
            {
                candidate.y = fallbackY;
            }

            GameObject prefab = seaweedPrefabs[Random.Range(0, seaweedPrefabs.Length)];
            Quaternion randomYaw = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            GameObject instance = Instantiate(prefab, candidate, randomYaw, transform);

            float lowScale = Mathf.Min(minScale, maxScale);
            float highScale = Mathf.Max(minScale, maxScale);
            float randomScale = Random.Range(lowScale, highScale);
            instance.transform.localScale *= randomScale;

            placedPositions.Add(candidate);
            spawned++;
        }

        if (spawned < spawnCount)
        {
            Debug.LogWarning("SeaweedScatterer: Could not place all seaweed instances. Increase area or reduce minSpacing.");
        }
    }

    [ContextMenu("Clear Spawned Seaweed")]
    public void ClearChildren()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                DestroyImmediate(child.gameObject);
            }
            else
            {
                Destroy(child.gameObject);
            }
#else
            Destroy(child.gameObject);
#endif
        }
    }

    private Vector3 GetRandomPositionXZ()
    {
        float x = Random.Range(-halfSizeX, halfSizeX);
        float z = Random.Range(-halfSizeZ, halfSizeZ);
        Vector3 worldCenter = transform.position;
        return new Vector3(worldCenter.x + x, worldCenter.y, worldCenter.z + z);
    }

    private bool IsFarEnough(Vector3 candidate)
    {
        float minSpacingSqr = minSpacing * minSpacing;

        for (int i = 0; i < placedPositions.Count; i++)
        {
            Vector3 placed = placedPositions[i];
            Vector2 candidateXZ = new Vector2(candidate.x, candidate.z);
            Vector2 placedXZ = new Vector2(placed.x, placed.z);

            if ((candidateXZ - placedXZ).sqrMagnitude < minSpacingSqr)
            {
                return false;
            }
        }

        return true;
    }
}
