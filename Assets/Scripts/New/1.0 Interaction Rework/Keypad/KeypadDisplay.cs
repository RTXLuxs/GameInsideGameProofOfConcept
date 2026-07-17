using TMPro;
using UnityEngine;

public class KeypadDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text displayText;

    public void SetText(string text)
    {
        displayText.text = text;
    }

    public void Clear()
    {
        displayText.text = "";
    }

    public void ShowMessage(string message)
    {
        displayText.text = message;
    }
}