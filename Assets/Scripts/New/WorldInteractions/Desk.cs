using UnityEngine;

public class Desk : MonoBehaviour, IInteractable
{
    public string GetInteractionText()
    {
        return "Use PC [E]";
    }

    public void Interact()
    {
        SwitchCameras.Instance.SwitchToDesk();
        PlayerState.Instance.EnterPC();
    }
}
