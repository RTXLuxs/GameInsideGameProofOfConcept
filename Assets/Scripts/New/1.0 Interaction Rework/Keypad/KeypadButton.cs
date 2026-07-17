using UnityEngine;

public class KeypadButton : MonoBehaviour
{
    public enum ButtonType
    {
        Digit,
        Enter,
        Backspace,
        Clear
    }

    [Header("References")]
    [SerializeField] private KeypadController controller;

    [Header("Button")]
    [SerializeField] private ButtonType buttonType;
    [SerializeField] private int digit;

    private void OnMouseDown()
    {
        switch (buttonType)
        {
            case ButtonType.Digit:
                controller.PressDigit(digit);
                break;

            case ButtonType.Enter:
                controller.Submit();
                break;

            case ButtonType.Backspace:
                controller.Backspace();
                break;

            case ButtonType.Clear:
                controller.Clear();
                break;
        }

        // TODO:
        // Play button press animation

        // TODO:
        // Play click sound
    }
}
