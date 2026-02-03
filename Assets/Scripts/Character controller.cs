using UnityEngine;

public class Mover : MonoBehaviour
{
 public float speed = 2;
 CharacterController cc;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 dir = Vector3.zero;
        Quaternion rot = transform.rotation;

        if (Input.GetKey("w"))
        {
            dir = new Vector3(0, 0, 1);
            rot = Quaternion.Euler(0, 0, 0);
        }
        else if (Input.GetKey("s"))
        {
            dir = new Vector3(0, 0, -1);
            rot = Quaternion.Euler(0, 180, 0);
        }
        else if (Input.GetKey("a"))
        {
            dir = new Vector3(-1, 0, 0);
            rot = Quaternion.Euler(0, -90, 0);
        }
        else if (Input.GetKey("d"))
        {
            dir = new Vector3(1, 0, 0);
            rot = Quaternion.Euler(0, 90, 0);
        }

        transform.rotation = rot;
        cc.SimpleMove(dir * speed);
    }
}
 
