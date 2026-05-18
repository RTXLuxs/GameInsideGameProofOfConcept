using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public Vector3 playerPos;
    public string mapBoundry; // Boundary name
    public List<InventorySaveData> inventorySaveData;
    public List<InventorySaveData> hotbarSaveData;
    public List<ContainerSaveData> containerSaveData;
    public int equippedItemID = -1; // -1 means no item equipped
}

[System.Serializable]
public class ContainerSaveData
{
    public string containerID;
    public bool isOpened;
}