using UnityEngine;

/// <summary>
/// A 3D appliance (e.g. a microwave) the player can switch on. It plays a sound in the 3D
/// world and lures the 2D enemy toward the paired 2D object, matched by <see cref="lureId"/>
/// (see <c>LureTarget2D</c>).
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class LureSoundInteractable : MonoBehaviour, IInteractable
{
    [Header("Interaction")]
    [SerializeField] private string interactionText = "Turn on Microwave [E]";

    [Header("Link")]
    [Tooltip("Must match the Lure Id on the paired 2D LureTarget2D.")]
    [SerializeField] private string lureId;

    [Header("Sound")]
    [Tooltip("The clip to play when switched on. The AudioSource is added automatically.")]
    [SerializeField] private AudioClip sound;

    [Header("Cooldown")]
    [Tooltip("Seconds before it can be triggered again.")]
    [SerializeField] private float cooldown = 1f;

    private AudioSource audioSource;
    private float lastUsedTime = -999f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public string GetInteractionText()
    {
        return interactionText;
    }

    public void Interact()
    {
        if (Time.time - lastUsedTime < cooldown)
            return;

        lastUsedTime = Time.time;

        PlaySound();
        LureEnemy();
    }

    private void PlaySound()
    {
        if (audioSource == null || sound == null)
            return;

        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.PlayOneShot(sound);
    }

    private void LureEnemy()
    {
        LureTarget2D target = LureTarget2D.Get(lureId);
        if (target == null)
        {
            Debug.LogWarning($"{name}: No 2D LureTarget2D registered for lureId '{lureId}'.", this);
            return;
        }

        DistractionSound.Emit(target.Position2D);
    }
}
