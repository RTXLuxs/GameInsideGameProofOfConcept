using UnityEngine;

public class PlayerProximity : MonoBehaviour
{
    [SerializeField] private Transform player;

    [Header("Warning")]
    [SerializeField] private float warningDistance = 20f;

    [Header("Death")]
    [SerializeField] private float killDistance = 3f;
    [SerializeField] private float killTime = 5f;

    private float deathTimer;

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        // Proximity vignette
        ScreenEffects.Instance.SetDangerProximityByDistance(distance, warningDistance);

        // Death fade
        if (distance <= killDistance)
        {
            deathTimer += Time.deltaTime;
        }
        else
        {
            deathTimer -= Time.deltaTime;
        }

        deathTimer = Mathf.Clamp(deathTimer, 0f, killTime);

        float progress = deathTimer / killTime;

        ScreenEffects.Instance.SetDeathFade(progress);

        if (progress >= 1f)
        {
            Debug.Log("Player Died");

            // TODO:
            // Kill player
        }
    }
}
