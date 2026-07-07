using UnityEngine;


[RequireComponent(typeof(WorldObject))]
public class OpenableState : MonoBehaviour
{
    private static readonly int OpenHash = Animator.StringToHash("Open");

    private WorldObject openableObject;
    private Animator openableAnimator;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;

    public bool IsOpen => openableObject.GetState() == WorldObjectState.Open;

    private void Awake()
    {
        openableObject = GetComponent<WorldObject>();
        openableAnimator = GetComponent<Animator>();

        if (openableObject == null)
        {
            Debug.LogError($"{name} requires a WorldObject component.", this);
        }
    }

    public void Open()
    {
        if (IsOpen)
            return;

        openableObject.SetState(WorldObjectState.Open);

        if (openableAnimator != null)
        {
            openableAnimator.SetBool(OpenHash, true);
        }

        if (audioSource != null && openSound != null)
        {
            audioSource.PlayOneShot(openSound);
        }
    }

    public void Close()
    {
        if (!IsOpen)
            return;

        openableObject.SetState(WorldObjectState.Closed);

        if (openableAnimator != null)
        {
            openableAnimator.SetBool(OpenHash, false);
        }

        if (audioSource != null && closeSound != null)
        {
            audioSource.PlayOneShot(closeSound);
        }
    }

    public void Toggle()
    {
        if (IsOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }
}
