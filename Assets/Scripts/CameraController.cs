using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
   
    void LateUpdate()
    {
        transform.position = new Vector3(player.transform.position.x, (player.transform.position.y + 1.5f), -8.5f);
    }
}
