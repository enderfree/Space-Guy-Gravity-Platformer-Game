using UnityEngine;

public class Character : MonoBehaviour
{
    public static float speed = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("a")) 
        {
            transform.position += Vector3.left * Time.deltaTime * speed;
        }
    }
}
