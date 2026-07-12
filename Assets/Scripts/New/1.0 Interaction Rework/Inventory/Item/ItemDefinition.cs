using UnityEngine;

[CreateAssetMenu(
    fileName = "New Item",
    menuName = "Bedridden/Inventory/Item")]
public class ItemDefinition : ScriptableObject
{
    [Header("General")]
    [SerializeField] private string itemID;
    [SerializeField] private string itemName;
    [SerializeField] private ItemType itemType;

    [Header("UI")]
    [SerializeField] private Sprite icon;

    [TextArea(2, 5)]
    [SerializeField] private string description;

    [Header("Settings")]
    [SerializeField] private bool importantItem = true;
    [SerializeField] private bool consumable = false;

    public string ItemID => itemID;

    public string ItemName => itemName;

    public ItemType ItemType => itemType;

    public Sprite Icon => icon;

    public string Description => description;

    public bool ImportantItem => importantItem;

    public bool Consumable => consumable;
}
