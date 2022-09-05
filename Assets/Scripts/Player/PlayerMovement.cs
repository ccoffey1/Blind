using Assets.Scripts.Enum;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Components
    Rigidbody2D rb;
    Footsteps footsteps;
    Player playerData;

    // Player
    [SerializeField]
    private float tipToeSpeed = 0.5f;
    [SerializeField]
    private float walkSpeed = 2f;
    [SerializeField]
    private float sprintSpeed = 3f;
    private float speedLimiter = 0.7f;
    private float turnSpeed = 6f;
    private float inputHorizontal;
    private float inputVertical;

    [HideInInspector]
    public bool PlayerIsWalking;

    private bool tipToeing;
    private bool sprinting;

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

        sprinting = Input.GetKey(KeyCode.LeftShift);
        tipToeing = !sprinting && Input.GetKey(KeyCode.LeftControl);

        footsteps.FootstepType = GetFootstepType();
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

            float moveSpeed = GetMoveSpeed();
            rb.velocity = new Vector2(inputHorizontal * moveSpeed, inputVertical * moveSpeed);

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

    private FootstepType GetFootstepType()
    {
        return sprinting
            ? FootstepType.Sprinting
                : tipToeing
                    ? FootstepType.TipToe
                        : FootstepType.Walking;
    }

    private float GetMoveSpeed()
    {
        return sprinting
            ? sprintSpeed
                : tipToeing
                    ? tipToeSpeed
                        : walkSpeed;
    }
}
