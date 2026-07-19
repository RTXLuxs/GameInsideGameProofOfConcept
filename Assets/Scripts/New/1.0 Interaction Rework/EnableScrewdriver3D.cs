using Unity.Cinemachine;
using UnityEngine;

public class EnableScrewdriver3D : MonoBehaviour
{
    private GameObject screwdriver3D;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        screwdriver3D = GameObject.Find("Screwdriver3D");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Screwdriver"))
        {
            screwdriver3D.SetActive(true);
            Debug.Log("this");
        }
    }
}
