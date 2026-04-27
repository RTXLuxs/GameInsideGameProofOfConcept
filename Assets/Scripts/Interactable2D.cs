using UnityEngine;

public class Interactable2D : MonoBehaviour
{
    private WorldObject worldObject;

    private void Start()
    {
        worldObject = GetComponent<WorldObject>();
    }

    public Transform player;
    public float range = 2f;

    public GameObject uiText;

    private void Update()
    {
        if (Vector2.Distance(transform.position, player.position) <= range)
        {
            uiText.SetActive(true);
        }
        else
        {
            uiText.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.E) && Vector2.Distance(transform.position, player.position) <= range)
        {
            Interact();
        }
    }

    public void Interact()
    {
        worldObject.SetState(WorldObjectState.Open);
        uiText.SetActive(false);
        Destroy(gameObject);
    }
}
