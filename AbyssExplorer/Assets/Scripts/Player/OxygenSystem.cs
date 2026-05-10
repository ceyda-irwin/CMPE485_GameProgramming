using UnityEngine;

public class OxygenSystem : MonoBehaviour
{
    [Header("Oxygen")]
    public float maxOxygen = 100f;
    public float currentOxygen;

    [Header("Rates")]
    public float oxygenDecreaseRate = 5f;
    public float oxygenRefillRate = 12f;

    [Header("Surface Settings")]
    public float surfaceYLevel = 20f;

    [Header("Environment Visuals")]
    public bool controlEnvironmentVisuals = true;
    public Camera targetCamera;
    public float visualTransitionSpeed = 4f;
    public Color underwaterFogColor = new Color(0.05f, 0.18f, 0.25f);
    public Color airFogColor = new Color(0.55f, 0.72f, 0.9f);
    public float underwaterFogDensity = 0.018f;
    public float airFogDensity = 0.002f;
    public Color underwaterCameraColor = new Color(0.08f, 0.18f, 0.24f);
    public Color airCameraColor = new Color(0.55f, 0.75f, 0.95f);

    [Header("Water Surface")]
    public bool createWaterSurface = true;
    public float waterSurfaceSize = 320f;
    public Color waterSurfaceColor = new Color(0.1f, 0.55f, 0.8f, 0.35f);

    [Header("State")]
    public bool isAboveSurface = false;

    private Renderer waterSurfaceRenderer;

    private void Start()
    {
        currentOxygen = maxOxygen;

        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        if (createWaterSurface)
        {
            CreateWaterSurface();
        }
    }

    private void Update()
    {
        if (GameManager.Instance == null) return;
        if (!GameManager.Instance.gameStarted || GameManager.Instance.gameEnded) return;

        CheckSurfaceState();
        UpdateOxygen();
        UpdateEnvironmentVisuals();
    }

    private void CheckSurfaceState()
    {
        isAboveSurface = transform.position.y >= surfaceYLevel;
    }

    private void UpdateOxygen()
    {
        if (isAboveSurface)
        {
            currentOxygen += oxygenRefillRate * Time.deltaTime;
        }
        else
        {
            currentOxygen -= oxygenDecreaseRate * Time.deltaTime;
        }

        currentOxygen = Mathf.Clamp(currentOxygen, 0f, maxOxygen);

        if (currentOxygen <= 0f)
        {
            GameManager.Instance.LoseGame();
        }
    }

    private void UpdateEnvironmentVisuals()
    {
        if (!controlEnvironmentVisuals) return;

        float targetFogDensity = isAboveSurface ? airFogDensity : underwaterFogDensity;
        Color targetFogColor = isAboveSurface ? airFogColor : underwaterFogColor;
        Color targetCameraColor = isAboveSurface ? airCameraColor : underwaterCameraColor;
        float blend = 1f - Mathf.Exp(-visualTransitionSpeed * Time.deltaTime);

        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, targetFogDensity, blend);
        RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, targetFogColor, blend);

        if (targetCamera != null)
        {
            targetCamera.backgroundColor = Color.Lerp(targetCamera.backgroundColor, targetCameraColor, blend);
        }
    }

    private void CreateWaterSurface()
    {
        GameObject waterSurface = GameObject.CreatePrimitive(PrimitiveType.Cube);
        waterSurface.name = "WaterSurface_Y20";
        waterSurface.transform.position = new Vector3(0f, surfaceYLevel, 0f);
        waterSurface.transform.localScale = new Vector3(waterSurfaceSize, 0.05f, waterSurfaceSize);

        Collider surfaceCollider = waterSurface.GetComponent<Collider>();
        if (surfaceCollider != null)
        {
            Destroy(surfaceCollider);
        }

        waterSurfaceRenderer = waterSurface.GetComponent<Renderer>();
        if (waterSurfaceRenderer == null) return;

        Material waterMaterial = new Material(Shader.Find("Standard"));
        waterMaterial.name = "Runtime Water Surface";
        waterMaterial.color = waterSurfaceColor;
        waterMaterial.SetFloat("_Mode", 3f);
        waterMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        waterMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        waterMaterial.SetInt("_ZWrite", 0);
        waterMaterial.DisableKeyword("_ALPHATEST_ON");
        waterMaterial.EnableKeyword("_ALPHABLEND_ON");
        waterMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        waterMaterial.renderQueue = 3000;

        waterSurfaceRenderer.material = waterMaterial;
        waterSurfaceRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        waterSurfaceRenderer.receiveShadows = false;
    }
}
