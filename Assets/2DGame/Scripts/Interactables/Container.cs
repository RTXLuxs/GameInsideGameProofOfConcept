using UnityEngine;

public class Container : MonoBehaviour, IInteractable2D
{
    public bool IsOpened { get; private set; }
    public string ContainerID { get; private set; }
    public GameObject itemPrefab;
    public Sprite openedSprite;
    private Sprite closedSprite;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        ContainerID ??= GlobalHelper.GenerateUniqueID(gameObject);
        closedSprite = GetComponent<SpriteRenderer>().sprite;
    }

    public bool CanInteract()
    {
        return !IsOpened;
    }

    public void Interact()
    {
        if (!CanInteract())
            return;
        OpenContainer();
    }

    private void OpenContainer()
    {
        SetOpened(true);

        if (itemPrefab)
        {
            // Drop the item slightly below the container to avoid overlap
            GameObject droppedItem = Instantiate(itemPrefab, transform.position + Vector3.down, Quaternion.identity);
        }
    }

    public void SetOpened(bool opened)
    {
        IsOpened = opened;
        GetComponent<SpriteRenderer>().sprite = IsOpened ? openedSprite : closedSprite;
    }
}
