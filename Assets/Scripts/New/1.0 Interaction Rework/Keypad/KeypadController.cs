using UnityEngine;

public class KeypadController : MonoBehaviour
{
    [Header("Keypad")]
    [SerializeField] private string correctCode = "1234";
    [SerializeField] private int maxCodeLength = 4;

    private KeypadInteraction keypadInteraction;
    private KeypadDisplay display;

    private string currentInput = "";

    private AudioSource audioSource;
    public AudioClip buttonClip;
    public AudioClip correctClip;
    public AudioClip wrongClip;

    private void Awake()
    {
        keypadInteraction = GetComponent<KeypadInteraction>();
        display = GetComponentInChildren<KeypadDisplay>();
        audioSource = GetComponent<AudioSource>();
        display.ShowMessage("Enter Code");
    }

    public void PressDigit(int digit)
    {
        if (currentInput.Length >= maxCodeLength)
            return;

        currentInput += digit.ToString();

        display.SetText(currentInput);

        audioSource.PlayOneShot(buttonClip);

        Debug.Log($"Current Input: {currentInput}");
    }

    public void Backspace()
    {
        if (currentInput.Length == 0)
            return;

        currentInput = currentInput.Substring(0, currentInput.Length - 1);

        display.SetText(currentInput);
        audioSource.PlayOneShot(buttonClip);

        Debug.Log($"Current Input: {currentInput}");
    }

    public void Clear()
    {
        currentInput = "";
        
        display.Clear();
        audioSource.PlayOneShot(buttonClip);

        Debug.Log("Input Cleared");
    }

    public bool Submit()
    {
        bool correct = currentInput == correctCode;

        if (correct)
        {
            Debug.Log("Correct Code");
            display.ShowMessage("Correct");
            audioSource.PlayOneShot(correctClip);
            keypadInteraction.ExitKeypad();
        }
        else
        {
            Debug.Log("Incorrect Code");
            audioSource.PlayOneShot(wrongClip);
            display.Clear();
        }

        currentInput = "";

        return correct;
    }

    public string GetCurrentInput()
    {
        return currentInput;
    }
}
