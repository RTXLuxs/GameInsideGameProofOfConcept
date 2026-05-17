using UnityEditor.Experimental.GraphView;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioLowPassFilter))]
public class AudioRoomOcclusion : MonoBehaviour
{
    [Header("References")]
    public ListenerRoomTracker listener;
    public SourceRoomTracker source;

    [Header("Portals")]
    private SoundPortal[] portals;

    private AudioSource audioSource;
    private AudioLowPassFilter lowPass;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        lowPass = GetComponent<AudioLowPassFilter>();

        portals = FindObjectsByType<SoundPortal>();
    }

    private void Update()
    {
        Room listenerRoom = listener.currentRoom;
        Room sourceRoom = source.currentRoom;

        if (listenerRoom == null ||
            sourceRoom == null)
            return;

        //If listener and source are in the same room
        if (listenerRoom == sourceRoom)
        {
            ApplyClearSound();
            return;
        }

        //Check if rooms are connected via the portals
        foreach (SoundPortal portal in portals)
        {
            if (
                portal.IsOpen() &&
                portal.Connects(
                    listenerRoom,
                    sourceRoom
                )
            )
            {
                ApplyMuffledSound();
                return;
            }
        }
        
        //No direct connection between the rooms
        ApplyHeavyMuffle();
    }

    //Clear sound and no effect applied
    private void ApplyClearSound()
    {
        audioSource.volume = 1f;
        lowPass.cutoffFrequency = 22000f;
    }

    //Slightly quieter sound used for direct connections with open doors
    private void ApplyMuffledSound()
    {
        audioSource.volume = 0.65f;
        lowPass.cutoffFrequency = 3500f;
    }

    //Very quiet sound used if rooms have no direct connection or doors are closed
    private void ApplyHeavyMuffle()
    {
        audioSource.volume = 0.35f;
        lowPass.cutoffFrequency = 1500f;
    }
}
