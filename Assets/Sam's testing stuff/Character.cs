using UnityEngine;

public class Character : MonoBehaviour
{
    public static float speed = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("a")) 
        {
            transform.position += Vector3.right * Time.deltaTime * speed;
        }
        else if (Input.GetKey("d"))
        {
            transform.position += Vector3.left * Time.deltaTime * speed;
        }

        //if (Input.GetKey("w") && gameObject.GetComponent<MeshCollider>().)
        //{
        //    transform.position += Vector3.right * Time.deltaTime * speed;
        //}
    }
}
