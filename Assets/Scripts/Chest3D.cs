using UnityEngine;

public class Chest3D : MonoBehaviour
{
    public string chestId;
    public GameObject lid;

    private void Update()
    {
        if (WorldState.Instance.GetState(chestId) == WorldObjectState.Open)
        {
            if (lid != null)
            {
                Destroy(lid); // remove lid in 3D world
            }
        }
    }
}
