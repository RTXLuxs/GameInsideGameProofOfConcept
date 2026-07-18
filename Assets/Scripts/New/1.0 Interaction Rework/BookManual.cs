using UnityEngine;

public class BookManual : MonoBehaviour
{
    AudioSource audioSource;
    Inspectable inspectable;
    Animator animator;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        inspectable = GetComponent<Inspectable>();
        animator = GetComponent<Animator>();
    }

    public void TriggerAfterFall()
    {
        audioSource.PlayOneShot(audioSource.clip);
        animator.enabled = false;
        inspectable.enabled = true;
    }
}
