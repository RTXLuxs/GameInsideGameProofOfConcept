using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [HideInInspector] public bool hasKey = false;

    [SerializeField] private string sceneToLoad = "Forouzan_2D-Minigame";

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
    }

    /// <summary>
    /// Restarts the whole game from scratch: tears down the persistent singletons and
    /// reloads the active (main 3D) scene, which also drops the additive 2D scene. A fresh
    /// GameManager then re-bootstraps everything, including reloading the 2D scene, so all
    /// state — including 2D health — starts over. This is the same restart used on 3D death.
    /// </summary>
    public void RestartGame()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Drop cross-scene world state so puzzle progress (doors, etc.) resets.
        if (WorldState.Instance != null)
            Destroy(WorldState.Instance.gameObject);
        WorldState.Instance = null;

        // Clear ourselves so the reloaded scene's GameManager bootstraps fresh.
        Instance = null;
        Destroy(gameObject);

        SceneManager.LoadScene(sceneIndex);
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
