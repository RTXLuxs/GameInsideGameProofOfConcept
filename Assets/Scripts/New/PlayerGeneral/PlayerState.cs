using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance;
    private PlayerMovement3D movement3D;
    private PlayerMovement movement2D;

    [HideInInspector] public bool isPCMode = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        movement3D = FindAnyObjectByType<PlayerMovement3D>();
    }

    private void Start()
    {
        movement2D = FindAnyObjectByType<PlayerMovement>();
    }

    public void EnterPC()
    {
        movement3D.DisableControls();
        movement2D.EnableControls();
        isPCMode = true;
    }

    public void ExitPC()
    {
        if (UserInput.instance.pausePressed)
        {
            SwitchCameras.Instance.SwitchToFPS();
            movement3D.EnableControls();
            movement2D.DisableControls();
            isPCMode = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ExitPC();
    }
}
