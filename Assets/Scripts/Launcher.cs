using UnityEngine;
using UnityEngine.InputSystem;

public class Launcher : MonoBehaviour
{
    private Vector2 worldPosition;
    private Vector2 direction;
    public InputSet inputSet;
    public GameObject bubblePrefab; //Add the bubble prefab 
    [SerializeField] private GameObject shootPoint; //Add the empty child where you want to shoot from
    [SerializeField] private GameObject armJoint;
    public float shotSpeed = 10f;
    private void OnEnable()
    {
        inputSet.Player.Attack.Enable();
        inputSet.Player.Attack.performed += onShootPerformed;
    }

    private void OnDisable()
    {
        inputSet.Player.Attack.performed -= onShootPerformed;
        inputSet.Player.Attack.Disable();
    }
    void Awake()
    {
        inputSet = new InputSet();
    }

    private void onShootPerformed(InputAction.CallbackContext context) 
    {
        Debug.Log("Bubble shot");
        GameObject bubbleShot = Instantiate(bubblePrefab, new Vector3(shootPoint.transform.position.x + 1f, shootPoint.transform.position.y, shootPoint.transform.position.z), shootPoint.transform.rotation );
        bubbleShot.GetComponent<Rigidbody2D>().linearVelocity = shootPoint.transform.right * shotSpeed;
    }  

    void Update()
    {
        ShootPointRotation();
    }

    private void ShootPointRotation()
    {
       
       //rotate towards mouse position
        worldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        direction = (worldPosition - (Vector2)armJoint.transform.position).normalized;
        armJoint.transform.right = direction;
        Debug.Log(direction);
    }
}
