using UnityEngine;

/// <summary>
/// Relay that lets 2D-scene UI (e.g. a game-over "Restart" button) trigger a full game
/// restart. The actual reload lives on the persistent <see cref="GameManager"/>, which
/// can't be referenced from another scene in the editor, so a button calls this instead.
/// </summary>
public class GameRestarter : MonoBehaviour
{
    /// <summary>Hook this to a Restart button's OnClick (or PlayerHealth.onGameOver).</summary>
    public void RestartGame()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.RestartGame();
        else
            Debug.LogWarning("GameRestarter: no GameManager instance found; cannot restart.", this);
    }
}
