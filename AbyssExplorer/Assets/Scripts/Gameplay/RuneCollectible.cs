using UnityEngine;

public class RuneCollectible : MonoBehaviour
{
    [Header("Visual")]
    public float rotateSpeed = 60f;
    public float floatHeight = 0.3f;
    public float floatSpeed = 2f;

    [Header("Feedback")]
    public ParticleSystem collectionEffect;
    public AudioClip collectSound;
    public float soundVolume = 0.7f;

    private Vector3 startPos;
    private static AudioClip defaultCollectSound;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        // Rotation
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);

        // Floating motion
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;

        transform.position = new Vector3(
            transform.position.x,
            newY,
            transform.position.z
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayFeedback();
        GameManager.Instance.CollectItem();

        Destroy(gameObject);
    }

    private void PlayFeedback()
    {
        if (collectionEffect != null)
        {
            ParticleSystem effect = Instantiate(collectionEffect, transform.position, Quaternion.identity);
            Destroy(effect.gameObject, 2f);
        }
        else
        {
            CreateDefaultCollectionEffect();
        }

        AudioClip clip = collectSound != null ? collectSound : GetDefaultCollectSound();
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, transform.position, soundVolume);
        }
    }

    private void CreateDefaultCollectionEffect()
    {
        GameObject effectObject = new GameObject("Rune Collection Effect");
        effectObject.transform.position = transform.position;

        ParticleSystem particles = effectObject.AddComponent<ParticleSystem>();
        particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        ParticleSystem.MainModule main = particles.main;
        main.duration = 0.35f;
        main.startLifetime = 0.55f;
        main.startSpeed = 3f;
        main.startSize = 0.18f;
        main.startColor = new Color(0.3f, 0.9f, 1f, 1f);
        main.maxParticles = 32;
        main.loop = false;

        ParticleSystem.EmissionModule emission = particles.emission;
        emission.rateOverTime = 0f;
        emission.SetBursts(new ParticleSystem.Burst[]
        {
            new ParticleSystem.Burst(0f, 24)
        });

        ParticleSystem.ShapeModule shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.25f;

        ParticleSystemRenderer renderer = particles.GetComponent<ParticleSystemRenderer>();
        Shader shader = Shader.Find("Sprites/Default");
        if (shader != null)
        {
            Material material = new Material(shader);
            material.color = new Color(0.3f, 0.9f, 1f, 1f);
            renderer.material = material;
        }

        particles.Play();
        Destroy(effectObject, 1.5f);
    }

    private AudioClip GetDefaultCollectSound()
    {
        if (defaultCollectSound != null) return defaultCollectSound;

        int sampleRate = 44100;
        float duration = 0.18f;
        int sampleCount = Mathf.CeilToInt(sampleRate * duration);
        float[] samples = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            float time = i / (float)sampleRate;
            float envelope = 1f - time / duration;
            float frequency = Mathf.Lerp(880f, 1320f, time / duration);
            samples[i] = Mathf.Sin(2f * Mathf.PI * frequency * time) * envelope * 0.35f;
        }

        defaultCollectSound = AudioClip.Create("Default Rune Collect", sampleCount, 1, sampleRate, false);
        defaultCollectSound.SetData(samples, 0);
        return defaultCollectSound;
    }
}
