using UnityEngine;

public class Player2DFootsteps : MonoBehaviour
{
    private AudioSource footstepAudioSource;

    [Header("Audio")]
    public AudioClip footstepClip;

    [Header("Settings")]
    public float stepDistance = 1.5f;

    private Vector2 lastStepPosition;

    private void Start()
    {
        lastStepPosition = transform.position;
        footstepAudioSource = GameObject.Find("FootstepProxy").GetComponent<AudioSource>();
    }

    private void Update()
    {
        Vector2 currentPos = transform.position;

        float distance = Vector2.Distance(
            currentPos,
            lastStepPosition
        );

        // Player moved enough for next step
        if (distance >= stepDistance)
        {
            PlayFootstep();

            lastStepPosition = currentPos;
        }
    }

    private void PlayFootstep()
    {
        footstepAudioSource.pitch =
            Random.Range(0.95f, 1.05f);

        footstepAudioSource.PlayOneShot(footstepClip);
    }
}
