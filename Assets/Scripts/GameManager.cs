using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [HideInInspector] public bool hasKey = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.LoadScene("2D-Minigame", LoadSceneMode.Additive);
    }

    public GameObject targetObjectInMainScene;

    public void DestroyMainSceneObject()
    {
        if (targetObjectInMainScene != null)
        {
            Destroy(targetObjectInMainScene);
        }
    }
}
