using UnityEngine;

public class JumpscareExtension : MonoBehaviour
{
    private AudioSource jumpscareAudioSource;
    private Animator animator;
    private Inspectable inspectable;

    private void Start()
    {
        jumpscareAudioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        inspectable = GetComponent<Inspectable>();
    }

    public void JumpscareAnimationEvent()
    {
        jumpscareAudioSource.PlayOneShot(jumpscareAudioSource.clip);
    }

    public void EndJumpscare()
    {
        animator.enabled = false;
        inspectable.enabled = true;
    }
}
