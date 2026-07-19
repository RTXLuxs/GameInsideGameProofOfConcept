using UnityEngine;

public class EnableFuseInteraction : MonoBehaviour
{
    public ItemPickup fusePickUp;

    public Collider fuseCollider;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player3D"))
        {
            fusePickUp.enabled = true;
            fuseCollider.enabled = true;
            Debug.Log("trigger enter");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player3D"))
        {
            fuseCollider.enabled=false;
            fusePickUp.enabled = false;
        }
    }
}
