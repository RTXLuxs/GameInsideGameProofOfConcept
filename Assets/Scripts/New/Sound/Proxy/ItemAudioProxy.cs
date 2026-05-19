using UnityEngine;

public class ItemAudioProxy : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(AudioClip clip)
    {
        audioSource.pitch =
            Random.Range(0.95f, 1.05f);

        audioSource.PlayOneShot(clip);

        Destroy(
            gameObject,
            clip.length + 0.1f
        );
    }
}
