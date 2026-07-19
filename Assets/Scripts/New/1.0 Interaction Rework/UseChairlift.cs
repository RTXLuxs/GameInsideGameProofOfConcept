using UnityEngine;

public class UseChairlift : MonoBehaviour, IInteractable
{
    public string liftText;

    public GameObject gameWonText;

    public string GetInteractionText()
    {
        return liftText;
    }

    public void Interact()
    {
        if (ChairliftState.Instance.fuseInserted && ChairliftState.Instance.codeEntered)
        {
            ScreenEffects.Instance.FadeOut(3f);
            gameWonText.SetActive(true);
            PlayerState.Instance.DisableControls();
        }
    }
}
