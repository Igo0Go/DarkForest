using UnityEngine;

[RequireComponent(typeof(CharacterController))]
//[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : PlayerPart
{
    [SerializeField, Range(1, 10), Tooltip("Скорость перемещения")] private float speed = 5f;
    [SerializeField, Range(1, 10), Tooltip("Скорость перемещения")] private float jumpHeight = 4;
    [SerializeField, Range(0, 2), Tooltip("Время перезарядки рывка")] private float sprintReloadTime = 0.3f;
    [SerializeField, Range(0, 2), Tooltip("Время рывка")] private float sprintTime = 0.3f;
    [SerializeField, Range(1, 3), Tooltip("Сила рывка")] private float sprintValue = 0.3f;


    [SerializeField, Range(-40, -1)]
    [Tooltip("Ограничение скорости падения. Это требуется, чтобы персонаж," +
        "падающий с большой высоты не проникал сквозь текстуры.")]
    private float terminalVelocity = -10.0f;
    [SerializeField, Range(0.1f, 5), Tooltip("Сила притяжения. g=1 - земная гравитация")] private float gravityMultiplier = 1f;


    private float jumpForce = 15.0f;
    private Vector3 moveVector;
    private float vertSpeed;
    private readonly float minFall = -1.5f;

    private const float grav = 50;

    private Transform myTransform;
    private float sprintMultiplier;
    private float currentSprintReloadTime;
    private bool useDash = false;
    private CharacterController characterController;
    private Vector3 lastPosition;
    private Quaternion lastRotation;

    public override void Activate()
    {
        jumpForce = Mathf.Sqrt(jumpHeight * 2 * grav * gravityMultiplier);
        myTransform = transform;
        vertSpeed = minFall;
        sprintMultiplier = 1;
        currentSprintReloadTime = 0;
        characterController = GetComponent<CharacterController>();
        FindObjectOfType<PlayerInteraction>().FallEvent += TeleportToLast;
    }

    private void Update()
    {
        vertSpeed -= gravityMultiplier * grav * Time.deltaTime;

        if(vertSpeed < terminalVelocity)
        {
            vertSpeed = terminalVelocity;
        }

        Jump();
        PlayerMove();
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && characterController.isGrounded)
        {
            lastPosition = transform.position;
            lastRotation = transform.rotation;
            vertSpeed = jumpForce;
        }
    }

    private void PlayerMove()
    {
        float deltaX = Input.GetAxisRaw("Horizontal");
        float deltaZ = Input.GetAxisRaw("Vertical");

        moveVector = myTransform.forward * deltaZ + myTransform.right * deltaX;
        moveVector.y = 0;
        moveVector = moveVector.normalized * speed * Time.deltaTime * sprintMultiplier;
        PlayerSprint(moveVector);
        if (sprintMultiplier == 1)
        {
            moveVector.y = vertSpeed * Time.deltaTime;
        }
        characterController.Move(moveVector);

        //rb.velocity = moveVector;
    }

    private void PlayerSprint(Vector3 horDirection)
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            if(horDirection.sqrMagnitude != 0 && !useDash)
            {
                sprintMultiplier = sprintValue;
                currentSprintReloadTime = 0;
                useDash = true;
            }
        }

        if (useDash)
        {
            currentSprintReloadTime += Time.deltaTime;

            if(currentSprintReloadTime > sprintTime)
            {
                sprintMultiplier = 1;
            }

            if(currentSprintReloadTime > sprintReloadTime)
            {
                useDash = false;
                currentSprintReloadTime = 0;
            }
        }
    }

    private void TeleportToLast()
    {
        characterController.enabled = false;
        transform.position = lastPosition;
        transform.rotation = lastRotation;
        characterController.enabled = true;
    }
}
