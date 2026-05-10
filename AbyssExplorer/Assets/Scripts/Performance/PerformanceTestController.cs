using TMPro;
using UnityEngine;

public class PerformanceTestController : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text performanceText;

    [Header("Particle Test")]
    public ParticleSystem bubbleParticles;

    [Header("Light Test")]
    public Light[] environmentLights;

    [Header("Fog Test")]
    public Color fogColor = new Color(0.05f, 0.18f, 0.25f);

    private int particleLevel = 1;
    private int lightLevel = 1;
    private int fogLevel = 1;

    private readonly string[] levelNames = { "Low", "Medium", "High" };

    private readonly int[] particleRateValues = { 30, 150, 600 };
    private readonly int[] maxParticleValues = { 300, 1500, 6000 };

    private readonly int[] activeLightCounts = { 1, 5, 12 };

    private readonly float[] fogDensityValues = { 0.003f, 0.008f, 0.018f };

    private void Start()
    {
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogColor = fogColor;

        ApplyAllSettings();
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        // Particle density
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            particleLevel = 0;
            ApplyParticleSettings();
            UpdateText();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            particleLevel = 1;
            ApplyParticleSettings();
            UpdateText();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            particleLevel = 2;
            ApplyParticleSettings();
            UpdateText();
        }

        // Light count
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            lightLevel = 0;
            ApplyLightSettings();
            UpdateText();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            lightLevel = 1;
            ApplyLightSettings();
            UpdateText();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            lightLevel = 2;
            ApplyLightSettings();
            UpdateText();
        }

        // Fog density
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            fogLevel = 0;
            ApplyFogSettings();
            UpdateText();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            fogLevel = 1;
            ApplyFogSettings();
            UpdateText();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            fogLevel = 2;
            ApplyFogSettings();
            UpdateText();
        }
    }

    private void ApplyAllSettings()
    {
        ApplyParticleSettings();
        ApplyLightSettings();
        ApplyFogSettings();
        UpdateText();
    }

    private void ApplyParticleSettings()
    {
        if (bubbleParticles == null) return;

        var emission = bubbleParticles.emission;
        emission.rateOverTime = particleRateValues[particleLevel];

        var main = bubbleParticles.main;
        main.maxParticles = maxParticleValues[particleLevel];

        bubbleParticles.Clear();
        bubbleParticles.Play();
    }

    private void ApplyLightSettings()
    {
        if (environmentLights == null) return;

        int targetActiveCount = activeLightCounts[lightLevel];

        for (int i = 0; i < environmentLights.Length; i++)
        {
            if (environmentLights[i] == null) continue;

            environmentLights[i].enabled = i < targetActiveCount;
        }
    }

    private void ApplyFogSettings()
    {
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogDensity = fogDensityValues[fogLevel];
        RenderSettings.fogColor = fogColor;
    }

    private void UpdateText()
    {
        if (performanceText == null) return;

        performanceText.text =
            "Performance Test Mode\n" +
            "1/2/3 Particles: " + levelNames[particleLevel] + "\n" +
            "4/5/6 Lights: " + levelNames[lightLevel] + "\n" +
            "7/8/9 Fog: " + levelNames[fogLevel] + "\n" +
            "F: Toggle FPS Counter";
    }
}