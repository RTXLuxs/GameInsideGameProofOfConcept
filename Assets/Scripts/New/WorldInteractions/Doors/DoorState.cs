using UnityEngine;

public class DoorState : MonoBehaviour
{
    private WorldObject thisObject;
    private Animator doorAnimator;

    public AudioSource audioSource;
    [SerializeField] private AudioClip doorOpeningSound;
    [SerializeField] private AudioClip doorClosingSound;

    private void Awake()
    {
        thisObject = GetComponent<WorldObject>();
        doorAnimator = GetComponent<Animator>();
        //audioSource = GetComponent<AudioSource>();
    }

    //Logic for opening door
    public void OpenDoor()
    {
        var currentState = thisObject.GetState();
        
        if (currentState == WorldObjectState.Open)
        {
            audioSource.PlayOneShot(doorOpeningSound);
            doorAnimator.SetBool("Open", true); //Plays opening animation in 2D/3D space
        }
    }

    //Logic for closing door
    public void CloseDoor()
    {
        var currentState = thisObject.GetState();

        if (currentState == WorldObjectState.Closed)
        {
            audioSource.PlayOneShot(doorClosingSound);
            doorAnimator.SetBool("Open", false); //Plays closing animation 2D/3D space
        }
    }

    private void Update()
    {

    }
}
