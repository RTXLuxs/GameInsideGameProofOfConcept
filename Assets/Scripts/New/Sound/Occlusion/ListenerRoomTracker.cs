using UnityEngine;

public class ListenerRoomTracker : MonoBehaviour
{
    //This script determines the room of the player/listener
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
