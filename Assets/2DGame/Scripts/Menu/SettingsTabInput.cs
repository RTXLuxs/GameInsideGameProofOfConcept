using UnityEngine;
using UnityEngine.UI;

public class SettingsTabInput : MonoBehaviour
{
    [SerializeField] private Button saveButton;
    [SerializeField] private Button resetButton;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) saveButton.onClick.Invoke();
        if (Input.GetKeyDown(KeyCode.P)) resetButton.onClick.Invoke();
    }
}
