using UnityEngine;

public class DoorState : MonoBehaviour
{
    private WorldObject thisObject;
    private Animator doorAnimator;

    private void Awake()
    {
        thisObject = GetComponent<WorldObject>();
        doorAnimator = GetComponent<Animator>();
    }

    //Logic for opening door
    private void OpenDoor()
    {
        var currentState = thisObject.GetState();
        
        if (currentState == WorldObjectState.Open)
        {
            doorAnimator.SetBool("Open", true); //Plays opening animation in 2D/3D space
        }
    }

    //Logic for closing door
    private void CloseDoor()
    {
        var currentState = thisObject.GetState();

        if (currentState == WorldObjectState.Closed)
        {
            doorAnimator.SetBool("Open", false); //Plays closing animation 2D/3D space
        }
    }

    private void Update()
    {
        OpenDoor();
        CloseDoor();
    }
}
