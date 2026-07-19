using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameSettings : MonoBehaviour
{
    bool menuOpen = false;

    public GameObject menuObj;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menuOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!menuOpen)
            {
                menuOpen = true;
                menuObj.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                menuObj.SetActive(false);
                menuOpen = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
