using UnityEngine;

public class ContainerState : MonoBehaviour
{
    private static readonly int OpenHash = Animator.StringToHash("Open");

    private WorldObject containerObject;
    private Animator containerAnimator;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;

    public bool IsOpen => containerObject.GetState() == WorldObjectState.Open;

    private void Awake()
    {
        containerObject = GetComponent<WorldObject>();
        containerAnimator = GetComponent<Animator>();
    }

    public void Open()
    {
        if (IsOpen)
            return;

        containerObject.SetState(WorldObjectState.Open);

        containerAnimator.SetBool(OpenHash, true);

        if (audioSource != null && openSound != null)
            audioSource.PlayOneShot(openSound);
    }

    public void Close()
    {
        if (!IsOpen)
            return;

        containerObject.SetState(WorldObjectState.Closed);

        containerAnimator.SetBool(OpenHash, false);

        if (audioSource != null && closeSound != null)
            audioSource.PlayOneShot(closeSound);
    }

    public void Toggle()
    {
        if (IsOpen)
            Close();
        else
            Open();
    }
}
