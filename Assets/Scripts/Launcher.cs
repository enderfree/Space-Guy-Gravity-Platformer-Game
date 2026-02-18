using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Launcher : MonoBehaviour
{
    private Vector2 worldPosition;
    private Vector2 direction;

    public InputSet inputSet;
    public GameObject bubblePrefab;
    [SerializeField] private GameObject shootPoint;
    [SerializeField] private GameObject armJoint;

    public EnergyBar energyBar;
    public int maxEnergy = 10;
    public int currentEnergy;

    public float shotSpeed = 10f;

  
    private GameObject currentBubble;

    private Camera cam;

    // ---------------- INPUT FIX ----------------
    void Awake()
    {
        if (inputSet == null)
            inputSet = new InputSet();

        cam = Camera.main;
    }

    private void OnEnable()
    {
        if (inputSet == null)
            inputSet = new InputSet();

        inputSet.Player.Attack.performed += onShootPerformed;
        inputSet.Player.Attack.Enable();
    }

    private void OnDisable()
    {
        inputSet.Player.Attack.performed -= onShootPerformed;
        inputSet.Player.Attack.Disable();
    }

    private void Start()
    {
        currentEnergy = maxEnergy;

        if (energyBar != null)
            energyBar.SetMaxEnergy(maxEnergy);
    }

    // ---------------- SHOOT ----------------
    private void onShootPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Shoot pressed");

        if (bubblePrefab == null || shootPoint == null)
        {
            Debug.LogWarning("Missing Bubble Prefab or ShootPoint!");
            return;
        }

        if (currentEnergy > 0)
        {
            currentEnergy -= 1;

            if (energyBar != null)
                energyBar.SetEnergy(currentEnergy);

            // destroy previous bubble
            if (currentBubble != null)
                Destroy(currentBubble);

            // spawn slightly in front of gun
            currentBubble = Instantiate(
                bubblePrefab,
                shootPoint.transform.position + shootPoint.transform.right * 0.6f,
                shootPoint.transform.rotation
            );

            Rigidbody2D rb = currentBubble.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.linearVelocity = shootPoint.transform.right * shotSpeed;

            // temporarily ignore player collision
            Collider2D bubbleCol = currentBubble.GetComponent<Collider2D>();
            Collider2D playerCol = GetComponent<Collider2D>();

            if (bubbleCol != null && playerCol != null)
            {
                Physics2D.IgnoreCollision(bubbleCol, playerCol, true);
                StartCoroutine(ReEnableCollision(bubbleCol, playerCol));
            }
        }
        else
        {
            Debug.Log("Out of ammo");
        }
    }

   
    private IEnumerator ReEnableCollision(Collider2D bubbleCol, Collider2D playerCol)
    {
        yield return new WaitForSeconds(0.25f);

        if (bubbleCol != null && playerCol != null)
            Physics2D.IgnoreCollision(bubbleCol, playerCol, false);
    }

    // ---------------- AIM ----------------
    void Update()
    {
        ShootPointRotation();
    }

    private void ShootPointRotation()
    {
        if (armJoint == null || cam == null)
            return;

      
        Vector3 mouseScreen = Mouse.current.position.ReadValue();
        mouseScreen.z = Mathf.Abs(cam.transform.position.z);
        worldPosition = cam.ScreenToWorldPoint(mouseScreen);

        direction = (worldPosition - (Vector2)armJoint.transform.position).normalized;
        armJoint.transform.right = direction;
    }
}
