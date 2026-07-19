using UnityEngine;

public class BookManual : MonoBehaviour
{
    AudioSource audioSource;
    Inspectable inspectable;
    Collider colliderBook;
    Animator animator;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        inspectable = GetComponent<Inspectable>();
        animator = GetComponent<Animator>();
        colliderBook = GetComponent<Collider>();
    }

    public void TriggerAfterFall()
    {
        audioSource.PlayOneShot(audioSource.clip);
        animator.enabled = false;
        colliderBook.enabled = true;
        inspectable.enabled = true;
    }
}
