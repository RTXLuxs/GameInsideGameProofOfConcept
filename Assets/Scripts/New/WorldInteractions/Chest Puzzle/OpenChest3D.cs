using UnityEngine;

public class OpenChest3D : MonoBehaviour
{
    private WorldObject thisObject;
    private Animator animator;
    private AudioSource audioSource;

    private void Awake()
    {
        thisObject = GetComponent<WorldObject>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        WorldState.Instance.OnStateChanged -= OnWorldStateChanged;
    }

    private void Start()
    {
        WorldState.Instance.OnStateChanged += OnWorldStateChanged;
    }

    private void OnWorldStateChanged(string objectId, WorldObjectState state)
    {
        // Ignore unrelated objects
        if (objectId != thisObject.objectId)
            return;

        if (state == WorldObjectState.Open)
        {
            animator.SetBool("Open", true);
            audioSource.Play();
        }
        else
        {
            return;
        }
    }
}
