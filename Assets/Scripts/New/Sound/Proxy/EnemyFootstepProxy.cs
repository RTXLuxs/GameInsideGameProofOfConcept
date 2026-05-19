using UnityEngine;

public class EnemyFootstepProxy : MonoBehaviour
{
    private Transform enemy2D;

    private void Start()
    {
        enemy2D = GameObject.Find("Enemy A*").transform;
        if (enemy2D == null)
        {
            Debug.LogError("Player2D not found in the scene. Please ensure there is a GameObject named 'Player2D' with a Transform component.");
        }
    }

    private void LateUpdate()
    {
        if (enemy2D == null) return;

        //Converts 2D position to 3D
        Vector2 pos2D = new Vector2(enemy2D.position.x, enemy2D.position.y);
        Vector3 pos3D = WorldMapper.Instance.ConvertTo3D(pos2D);

        transform.position = pos3D;
    }
}
