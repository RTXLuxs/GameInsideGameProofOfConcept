using UnityEngine;

public class FootstepProxy : MonoBehaviour
{
    private Transform player2D;

    private void Start()
    {
        player2D = GameObject.Find("Player2D").transform;
    }

    private void LateUpdate()
    {
        if (player2D == null) return;

        //Converts 2D position to 3D
        Vector2 pos2D = new Vector2(player2D.position.x, player2D.position.y);
        Vector3 pos3D = WorldMapper.Instance.ConvertTo3D(pos2D);

        transform.position = pos3D;
    }
}
