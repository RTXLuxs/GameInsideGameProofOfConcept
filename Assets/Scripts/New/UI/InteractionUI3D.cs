using UnityEngine;
using TMPro;

public class InteractionUI3D : MonoBehaviour
{
    public static InteractionUI3D Instance;

    public GameObject panel;
    public TMPro.TMP_Text interactText;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void ShowText(string msg)
    {
        panel.SetActive(true);
        interactText.text = msg;
    }

    public void HideText()
    {
        panel.SetActive(false);
    }
}
