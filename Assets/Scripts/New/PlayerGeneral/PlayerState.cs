using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance;
    private PlayerMovementWheelchair movement3D;
    private PlayerMovement movement2D;

    [HideInInspector] public bool isPCMode = false;

    [HideInInspector] public bool bedridden = true;

    [HideInInspector] public bool canUseTablet = false;

    [HideInInspector] public bool carriesScrewdriver = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        movement3D = FindAnyObjectByType<PlayerMovementWheelchair>();
    }

    private void Start()
    {
        movement2D = FindAnyObjectByType<PlayerMovement>();
    }

    public void EnterPC()
    {
        SwitchCameras.Instance.SwitchToDesk();
        movement3D.DisableControls();
        movement2D.EnableControls();
        isPCMode = true;
    }

    public void ExitPC()
    {
        SwitchCameras.Instance.SwitchToFPS();
        if (!bedridden)
        {
            movement3D.EnableControls();
        }else if (bedridden)
        {
            movement3D.LookOnly();
        }
        movement2D.DisableControls();
        isPCMode = false;
    }

    public void DisableControls()
    {
        movement3D.DisableControls();
        movement2D.DisableControls();
    }

    public void EnableControls()
    {
        movement3D.EnableControls();
    }

    public void LookOnly()
    {
        movement3D.LookOnly();
    }

    // Update is called once per frame
    void Update()
    {
        //ExitPC();
    }
}
