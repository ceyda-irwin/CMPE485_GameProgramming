using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text fpsText;

    [Header("Settings")]
    public float updateInterval = 0.5f;

    private float timer;
    private int frameCount;
    private bool visible = true;

    private void Start()
    {
        if (fpsText != null)
        {
            fpsText.gameObject.SetActive(visible);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            visible = !visible;

            if (fpsText != null)
            {
                fpsText.gameObject.SetActive(visible);
            }
        }

        timer += Time.unscaledDeltaTime;
        frameCount++;

        if (timer >= updateInterval)
        {
            float fps = frameCount / timer;

            if (fpsText != null)
            {
                fpsText.text = "FPS: " + Mathf.RoundToInt(fps);
            }

            timer = 0f;
            frameCount = 0;
        }
    }
}