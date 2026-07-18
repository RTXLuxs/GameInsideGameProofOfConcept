using DoorScript;
using UnityEngine;

public class JumpscareEventTrigger : MonoBehaviour
{
    public Animator jumpscareAnimator;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player3D"))
            return;

        jumpscareAnimator.SetTrigger("Jumpscare");
    }

    public void JumpscareEvent()
    {

    }
}
