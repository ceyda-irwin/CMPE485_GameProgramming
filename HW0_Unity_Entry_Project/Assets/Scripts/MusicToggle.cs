using UnityEngine;

public class MusicToggle : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    private void Reset()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && audioSource != null)
        {
            if (audioSource.isPlaying) audioSource.Pause();
            else audioSource.UnPause();
        }
    }
}