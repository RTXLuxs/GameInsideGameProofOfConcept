using UnityEngine;

public class SimplePlayerMovement : MonoBehaviour
{

    [SerializeField] private float speed = 4f;
    public bool canMove = true;

    SwitchCameras switchCameras;
    MouseLook mouseLook;

    private void Start()
    {
        switchCameras = GetComponent<SwitchCameras>();
        mouseLook = GetComponentInChildren<MouseLook>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove) return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = (transform.forward * vertical + transform.right * horizontal);
        transform.position += move * speed * Time.deltaTime;
    }
}
