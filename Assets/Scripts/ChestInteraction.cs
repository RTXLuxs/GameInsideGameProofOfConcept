using UnityEngine;

public class ChestInteraction : MonoBehaviour
{
    public Transform player;
    public float range = 2f;

    public GameObject chestLid;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) &&
            Vector2.Distance(transform.position, player.position) <= range)
        {
            GameManager.Instance.DestroyMainSceneObject();
            gameObject.SetActive(false);
        }
    }
}
