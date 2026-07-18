using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Tracks the 2D player's health/lives. Every death deducts one point; while points
/// remain the player respawns, and when they hit zero it's game over.
///
/// <see cref="Die"/> is the single death entry point — enemies (and anything else that
/// kills the player) call it instead of respawning directly.
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [Tooltip("Number of deaths the player can take. At 0 remaining it's game over (default 3).")]
    [SerializeField] private int maxHealth = 3;

    [Header("UI (optional)")]
    [Tooltip("Life/heart icons, in order. Icons at or beyond the current health are hidden as HP drops.")]
    [SerializeField] private GameObject[] hearts;
    [Tooltip("Shown when the player runs out of health.")]
    [SerializeField] private GameObject gameOverUI;

    [Header("Events")]
    [Tooltip("Fired after a death that still leaves the player alive (a respawn follows).")]
    public UnityEvent onDamaged;
    [Tooltip("Fired once when health reaches zero.")]
    public UnityEvent onGameOver;

    public int CurrentHealth { get; private set; }
    public int MaxHealth => maxHealth;
    public bool IsGameOver { get; private set; }

    private PlayerMovement movement;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        CurrentHealth = maxHealth;
    }

    private void Start()
    {
        if (gameOverUI != null)
            gameOverUI.SetActive(false);
        UpdateHearts();
    }

    /// <summary>
    /// Call when the player is killed. Deducts one health point; respawns if any remain,
    /// otherwise ends the game.
    /// </summary>
    public void Die()
    {
        if (IsGameOver) return;

        CurrentHealth = Mathf.Max(0, CurrentHealth - 1);
        UpdateHearts();

        if (CurrentHealth <= 0)
        {
            GameOver();
        }
        else
        {
            onDamaged?.Invoke();
            if (movement != null)
                movement.Respawn();
        }
    }

    /// <summary>Restores full health and hides the game-over UI (e.g. for a Restart button).</summary>
    public void ResetHealth()
    {
        IsGameOver = false;
        CurrentHealth = maxHealth;
        if (gameOverUI != null)
            gameOverUI.SetActive(false);
        UpdateHearts();
    }

    private void GameOver()
    {
        IsGameOver = true;
        onGameOver?.Invoke();

        if (gameOverUI != null)
            gameOverUI.SetActive(true);
        if (movement != null)
            movement.DisableControls();
    }

    private void UpdateHearts()
    {
        if (hearts == null) return;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] != null)
                hearts[i].SetActive(i < CurrentHealth);
        }
    }
}
