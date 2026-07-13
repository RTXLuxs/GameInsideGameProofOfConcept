using Unity.Cinemachine;
using UnityEngine;

public class SwitchCameras : MonoBehaviour
{
    public static SwitchCameras Instance;

    public CinemachineCamera fpsCam;
    public CinemachineCamera deskCam;
    public CinemachineCamera cutsceneCam;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SwitchToDesk()
    {
        fpsCam.Priority = 0;
        deskCam.Priority = 10;
        cutsceneCam.Priority = 0;
    }

    public void SwitchToFPS()
    {
        fpsCam.Priority = 10;
        deskCam.Priority = 0;
        cutsceneCam.Priority = 0;
    }

    public void SwitchToCutscene()
    {
        fpsCam.Priority = 0;
        deskCam.Priority = 0;
        cutsceneCam.Priority = 10;
    }
}
