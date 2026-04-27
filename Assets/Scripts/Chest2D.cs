using UnityEngine;

public class Chest2D : MonoBehaviour
{
    public string chestId;

    public Transform player;
    public float range = 2f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && Vector2.Distance(transform.position, player.position) <= range)
        {
            Interact();
        }
    }

    public void Interact()
    {
        WorldState.Instance.SetState(chestId, WorldObjectState.Open);

        Destroy(gameObject); // remove 2D sprite
    }
}
