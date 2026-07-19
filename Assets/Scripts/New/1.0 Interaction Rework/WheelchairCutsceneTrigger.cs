using UnityEngine;

public class WheelchairCutsceneTrigger : MonoBehaviour
{
    [Header("Animations")]
    [SerializeField] private Animator wheelchairAnimator;
    [SerializeField] private Animator cameraAnimator;
    public Animator tabletAnimator;

    [Header("Animation State")]
    [SerializeField] private string animationNameWheelchair = "Intro";
    [SerializeField] private string animationNameCamera = "Intro";

    private bool triggered = false;

    private void Start()
    {
        wheelchairAnimator.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered)
            return;

        if (!other.CompareTag("Wheelchair"))
            return;

        triggered = true;

        PlayerState.Instance.canUseTablet = false;
        tabletAnimator.SetBool("Watching", false);
        PlayerState.Instance.ExitPC();
        SwitchCameras.Instance.SwitchToCutscene();
        wheelchairAnimator.enabled = true;
        wheelchairAnimator.Play(animationNameWheelchair, 0, 0f);
        cameraAnimator.Play(animationNameCamera, 0, 0f);

        Debug.Log("Intro cutscene started.");
    }
}
