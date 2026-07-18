using DoorScript;
using UnityEngine;

public class JumpscareEventTrigger : MonoBehaviour
{
    public Animator jumpscareAnimator;
    public AudioSource jumpscareAudioSource;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player3D"))
            return;

        if (jumpscareAnimator != null)
        {
            jumpscareAnimator.SetTrigger("Jumpscare");
        }
        
        JumpscareEvent();
    }

    public void JumpscareEvent()
    {
        if (jumpscareAudioSource != null)
        {
            jumpscareAudioSource.enabled = true;
        }
    }
}
