using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Components
    Rigidbody2D rb;
    Footsteps footsteps;
    Player playerData;

    // Player
    [SerializeField]
    private float walkSpeed = 2f;
    private float speedLimiter = 0.7f;
    private float turnSpeed = 6f;
    private float inputHorizontal;
    private float inputVertical;

    [HideInInspector]
    public bool PlayerIsWalking;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        footsteps = gameObject.GetComponent<Footsteps>();
        playerData = gameObject.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        if (inputHorizontal != 0f || inputVertical != 0f)
        {
            footsteps.Active = true;
            if (inputHorizontal != 0 && inputVertical != 0)
            {
                inputHorizontal *= speedLimiter;
                inputVertical *= speedLimiter;
            }
            rb.velocity = new Vector2(inputHorizontal * walkSpeed, inputVertical * walkSpeed);

            Quaternion currentRotation = transform.rotation;
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            Quaternion wantedRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(currentRotation, wantedRotation, Time.deltaTime * turnSpeed);
        }
        else
        {
            footsteps.Active = false;
            rb.velocity = Vector2.zero;
        }
    }
}
