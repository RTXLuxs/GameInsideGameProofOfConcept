using System;
using UnityEngine;

public class ThrownItem : MonoBehaviour
{
    public static event Action<Vector2> Landed;

    private Vector2 target;
    private float speed;
    private LayerMask obstacleLayer;
    private Collider2D col;
    private bool landed;

    [HideInInspector] public GameObject audioProxy;
    [HideInInspector] public AudioClip impactClip;

    public void Init(Vector2 target, float speed, LayerMask obstacleLayer)
    {
        this.target = target;
        this.speed = speed;
        this.obstacleLayer = obstacleLayer;
        col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
    }

    void Update()
    {
        if (landed) return;

        Vector2 pos = transform.position;
        Vector2 toTarget = target - pos;
        float distToTarget = toTarget.magnitude;
        float step = speed * Time.deltaTime;

        if (distToTarget > 0.05f)
        {
            Vector2 dir = toTarget / distToTarget;
            RaycastHit2D hit = Physics2D.Raycast(pos, dir, Mathf.Min(step, distToTarget), obstacleLayer);
            if (hit.collider != null)
            {
                transform.position = hit.point;
                OnLand();
                return;
            }
        }

        transform.position = Vector2.MoveTowards(pos, target, step);

        if (distToTarget <= step)
            OnLand();
    }

    private void OnLand()
    {
        landed = true;

        if (col != null)
            col.enabled = true;

        // Spawn impact sound
        PlayImpactSound();

        // Existing event
        Landed?.Invoke(transform.position);

        Destroy(this);
    }

    private void PlayImpactSound()
    {
        Vector2 pos2D = transform.position;

        Vector3 pos3D =
            WorldMapper.Instance.ConvertTo3D(pos2D);

        GameObject emitter =
            Instantiate(
                audioProxy,
                pos3D,
                Quaternion.identity
            );

        emitter
            .GetComponent<ItemAudioProxy>()
            .Play(impactClip);
    }
}
