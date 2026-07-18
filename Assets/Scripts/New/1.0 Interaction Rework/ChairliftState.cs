using UnityEngine;

public class ChairliftState : MonoBehaviour
{
    public static ChairliftState Instance;

    [HideInInspector] public bool fuseInserted = false;
    [HideInInspector] public bool codeEntered = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
