using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AutomaticDoorTrigger : MonoBehaviour
{
    [SerializeField] private OpenableState door;

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player3D") && !other.CompareTag("Wheelchair"))
            return;

        door.Open();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player3D") && !other.CompareTag("Wheelchair"))
            return;

        door.Close();
    }
}
