using UnityEngine;

public class CutsceneSwitchCams : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SwitchCameras.Instance.SwitchToCutscene();
        PlayerState.Instance.DisableControls();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchToFPSCam()
    {
        SwitchCameras.Instance.SwitchToFPS();
        PlayerState.Instance.LookOnly();
    }

    public void EnableTablet()
    {
        PlayerState.Instance.canUseTablet = true;
    }
}
