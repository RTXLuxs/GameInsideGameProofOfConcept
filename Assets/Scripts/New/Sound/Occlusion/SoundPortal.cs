using UnityEngine;

public class SoundPortal : MonoBehaviour
{
    //This script checks if rooms are connected and doors are opened/exist
    public Room roomA;
    public Room roomB;

    private WorldObject worldObject;

    private void Awake()
    {
        TryGetComponent(out worldObject);
    }

    //Check if pathway is blocked (by a door)
    public bool IsOpen()
    {
        if (worldObject == null)
        {
            return true; //No door = no block
        }


        return worldObject.GetState() == WorldObjectState.Open; //If door is open = true
    }

    //Check if listener and source rooms are connected
    public bool Connects(Room a, Room b)
    {
        return
            (a == roomA && b == roomB) ||
            (a == roomB && b == roomA);
    }
}
