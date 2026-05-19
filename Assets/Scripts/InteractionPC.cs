using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class InteractionPC : MonoBehaviour
{
    SwitchCameras switchCameras;
    MouseLook mouseLook;
    SimplePlayerMovement playerMovement;
    PlayerMovement2D playerMovement2D;
    PlayerMovement minigamePlayerMovement;

    public GameObject uiText;

    public Transform player;
    public Transform pc;
    public float range = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement = FindAnyObjectByType<SimplePlayerMovement>();
        playerMovement2D = FindAnyObjectByType<PlayerMovement2D>();
        minigamePlayerMovement = FindAnyObjectByType<PlayerMovement>();
        mouseLook = FindAnyObjectByType<MouseLook>();
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
            uiText.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.E) && Vector3.Distance(pc.position, player.position) <= range)
        {
            switchCameras.SwitchToDesk();
            mouseLook.canLook = false;
            playerMovement.canMove = false;
            playerMovement2D.canMove = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switchCameras.SwitchToFPS();
            mouseLook.canLook = true;
            playerMovement.canMove = true;
            playerMovement2D.canMove = false;
            if (minigamePlayerMovement != null) minigamePlayerMovement.DisableControls();
        }
    }
}
