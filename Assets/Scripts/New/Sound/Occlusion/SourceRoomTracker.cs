using UnityEngine;

public class SourceRoomTracker : MonoBehaviour
{
    //This script determines the room of the audio source
    public Room currentRoom {  get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        Room room = other.GetComponent<Room>();

        if (room != null)
        {
            currentRoom = room;
        }
    }
}
