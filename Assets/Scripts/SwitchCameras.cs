using Unity.Cinemachine;
using UnityEngine;

public class SwitchCameras : MonoBehaviour
{
    public CinemachineCamera fpsCam;
    public CinemachineCamera deskCam;

    public void SwitchToDesk()
    {
        fpsCam.Priority = 0;
        deskCam.Priority = 10;
    }

    public void SwitchToFPS()
    {
        fpsCam.Priority = 10;
        deskCam.Priority = 0;
    }
}
