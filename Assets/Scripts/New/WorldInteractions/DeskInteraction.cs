using TMPro;
using UnityEngine;

public class DeskInteraction : MonoBehaviour
{
    SwitchCameras switchCameras;
    PlayerMovementWheelchair playerMovement3D;
    PlayerMovement2D playerMovement2D;

    public GameObject uiText;

    public Transform player;
    public Transform pc;
    public float range = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement3D = FindAnyObjectByType<PlayerMovementWheelchair>();
        playerMovement2D = FindAnyObjectByType<PlayerMovement2D>();
        switchCameras = FindAnyObjectByType<SwitchCameras>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(pc.position, player.position) <= range && !playerMovement2D.canMove)
        {
            uiText.SetActive(true);
        }
        else
        {
            //uiText.SetActive(false);
        }

        if (UserInput.instance.interactPressed && Vector3.Distance(pc.position, player.position) <= range)
        {
            switchCameras.SwitchToDesk();
            playerMovement3D.DisableControls();
            playerMovement2D.canMove = true;
        }

        if (UserInput.instance.pausePressed)
        {
            switchCameras.SwitchToFPS();
            playerMovement3D.EnableControls();
            playerMovement2D.canMove = false;
        }
    }
}
