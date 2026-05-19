using UnityEngine;

public class KeyInteraction : MonoBehaviour, IInteractable
{

    //private WorldObject thisObject;

    private void Awake()
    {
        //thisObject = GetComponent<WorldObject>();
    }

    // Called when player interacts with this object
    public void Interact()
    {
        GameManager.Instance.hasKey = true;
        Debug.Log("Has key now");
        Destroy(gameObject);
    }

    // Text shown when player looks at the object
    public string GetInteractionText()
    {
        return "Pick up KEY [E]";
    }
}
