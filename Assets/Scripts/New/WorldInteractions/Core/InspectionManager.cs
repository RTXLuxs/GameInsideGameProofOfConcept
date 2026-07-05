using System.Collections;
using UnityEngine;

public class InspectionManager : MonoBehaviour
{
    public static InspectionManager Instance;

    [Header("References")]
    [SerializeField] private Transform inspectPoint;
    [SerializeField] private Transform rotationPivot;
    [SerializeField] private PlayerMovementWheelchair player;

    [Header("Settings")]
    [SerializeField] private float moveDuration = 0.35f;
    [SerializeField] private float rotationSpeed = 150f;

    private Transform cameraTransform;

    private Inspectable currentItem;
    private Transform currentTransform;

    private bool inspecting;
    private bool isTransitioning;

    private class InspectionState
    {
        public Transform parent;
        public Vector3 position;
        public Quaternion rotation;

        public Rigidbody rigidbody;
        public Collider[] colliders;
    }

    private InspectionState state;

    private void Awake()
    {
        Instance = this;
        cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        if (!inspecting || isTransitioning || currentItem == null)
            return;

        RotateObject();

        if (UserInput.Instance.interactPressed ||
            UserInput.Instance.pausePressed)
        {
            StopInspection();
        }
    }

    public void StartInspection(Inspectable item)
    {
        if (inspecting || isTransitioning || item == null)
            return;

        StartCoroutine(BeginInspection(item));
    }

    private IEnumerator BeginInspection(Inspectable item)
    {
        isTransitioning = true;
        inspecting = true;

        currentItem = item;
        currentTransform = item.transform;

        rotationPivot.localRotation = Quaternion.identity;

        player.DisableControls();

        state = new InspectionState
        {
            parent = currentTransform.parent,
            position = currentTransform.position,
            rotation = currentTransform.rotation,
            rigidbody = item.GetComponent<Rigidbody>(),
            colliders = item.GetComponentsInChildren<Collider>()
        };

        if (state.rigidbody != null)
        {
            state.rigidbody.isKinematic = true;
            state.rigidbody.linearVelocity = Vector3.zero;
            state.rigidbody.angularVelocity = Vector3.zero;
        }

        foreach (Collider c in state.colliders)
            c.enabled = false;

        Vector3 startPos = currentTransform.position;
        Quaternion startRot = currentTransform.rotation;

        float timer = 0;

        while (timer < moveDuration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / moveDuration);

            currentTransform.position = Vector3.Lerp(
                startPos,
                inspectPoint.position,
                t);

            currentTransform.rotation = Quaternion.Slerp(
                startRot,
                inspectPoint.rotation,
                t);

            yield return null;
        }

        currentTransform.SetParent(rotationPivot);

        currentTransform.localPosition = Vector3.zero;
        currentTransform.localRotation = Quaternion.identity;

        isTransitioning = false;
    }

    private void RotateObject()
    {
        Vector2 input = Vector2.zero;

        if (UserInput.Instance.isGamepad)
        {
            input = UserInput.Instance.aimInput;
        }
        else
        {
            if (!UserInput.Instance.mouseInteractHeld)
                return;

            input = UserInput.Instance.aimInput;
        }

        rotationPivot.Rotate(
            cameraTransform.up,
            input.x * rotationSpeed * Time.deltaTime,
            Space.World);

        rotationPivot.Rotate(
            cameraTransform.right,
            -input.y * rotationSpeed * Time.deltaTime,
            Space.World);
    }

    public void StopInspection()
    {
        if (!inspecting || isTransitioning)
            return;

        StartCoroutine(ReturnItem());
    }

    private IEnumerator ReturnItem()
    {
        isTransitioning = true;

        currentTransform.SetParent(null);

        Vector3 startPos = currentTransform.position;
        Quaternion startRot = currentTransform.rotation;

        float timer = 0;

        while (timer < moveDuration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / moveDuration);

            currentTransform.position = Vector3.Lerp(
                startPos,
                state.position,
                t);

            currentTransform.rotation = Quaternion.Slerp(
                startRot,
                state.rotation,
                t);

            yield return null;
        }

        currentTransform.SetParent(state.parent);

        if (state.rigidbody != null)
            state.rigidbody.isKinematic = false;

        foreach (Collider c in state.colliders)
            c.enabled = true;

        rotationPivot.localRotation = Quaternion.identity;

        player.EnableControls();

        currentItem = null;
        currentTransform = null;
        state = null;

        inspecting = false;
        isTransitioning = false;
    }
}
