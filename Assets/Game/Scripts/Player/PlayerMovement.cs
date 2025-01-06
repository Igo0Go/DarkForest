using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : PlayerPart
{
    [SerializeField, Range(1, 10), Tooltip("Скорость перемещения")] private float speed = 5f;
    [SerializeField, Range(1, 50), Tooltip("Сила прыжка")] private float jumpForce = 15.0f;
    [SerializeField, Range(0, 2), Tooltip("Время перезарядки рывка")] private float sprintReloadTime = 0.3f;
    [SerializeField, Range(0, 2), Tooltip("Время рывка")] private float sprintTime = 0.3f;
    [SerializeField, Range(1, 3), Tooltip("Сила рывка")] private float sprintValue = 0.3f;


    [SerializeField, Range(-40, -1)]
    [Tooltip("Ограничение скорости падения. Это требуется, чтобы персонаж," +
        "падающий с большой высоты не проникал сквозь текстуры.")]
    private float terminalVelocity = -10.0f;
    [SerializeField, Range(0.1f, 5), Tooltip("Сила притяжения. g=1 - земная гравитация")] private float gravity = 1f;
    [SerializeField] private Transform jumpCheck;
    [SerializeField] private LayerMask ignoreMask;

    private Vector3 moveVector;
    private float vertSpeed;
    private bool fall;
    private float fallTimer;
    private readonly float minFall = -1.5f;

    /// <summary>
    /// Этот коэффициент используется, чтобы добиться ощущения "правильной" гравитации при gravity = 1.
    /// </summary>
    private const float gravMultiplayer = 9.8f * 5f;

    private Transform myTransform;
    private float sprintMultiplicator;
    private float currentSprintReloadTime;
    private bool useDash = false;
    private Rigidbody rb;

    public override void Activate()
    {
        myTransform = transform;
        vertSpeed = minFall;
        fall = true;
        sprintMultiplicator = 1;
        currentSprintReloadTime = 0;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Jump();
        PlayerMove();
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            fallTimer = 0;
            fall = true;
            if (Input.GetButtonDown("Jump"))
            {
                vertSpeed = jumpForce;
            }
            else
            {
                vertSpeed = 0;
            }
        }
        else
        {
            if (fall)
            {
                vertSpeed -= gravity * gravMultiplayer * Time.deltaTime;
                if (vertSpeed < terminalVelocity)
                {
                    vertSpeed = terminalVelocity;
                }
            }
            else
            {
                fallTimer -= Time.deltaTime;
                if (fallTimer <= 0)
                {
                    fallTimer = 0;
                    fall = true;
                }
                vertSpeed = 0;
            }
        }
    }
    private void PlayerMove()
    {
        float deltaX = Input.GetAxisRaw("Horizontal");
        float deltaZ = Input.GetAxisRaw("Vertical");

        moveVector = myTransform.forward * deltaZ + myTransform.right * deltaX;
        moveVector.y = 0;
        moveVector = moveVector.normalized * speed;
        PlayerSprint(moveVector);
        if (sprintMultiplicator == 1)
        {
            moveVector.y = vertSpeed;
        }
        moveVector *= Time.deltaTime;

        rb.MovePosition(myTransform.position + moveVector * sprintMultiplicator);

        //myTransform.position += moveVector * sprintMultiplicator;
    }

    private bool IsGrounded()
    {
        Collider[] bufer = Physics.OverlapBox(jumpCheck.position, jumpCheck.localScale, Quaternion.identity, ~ignoreMask);

        if (bufer != null && bufer.Length > 0)
            return true;
        else
            return false;
    }

    private void PlayerSprint(Vector3 horDirection)
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            if(horDirection.sqrMagnitude != 0 && !useDash)
            {
                sprintMultiplicator = sprintValue;
                currentSprintReloadTime = 0;
                fall = false;
                useDash = true;
            }
        }

        if (useDash)
        {
            currentSprintReloadTime += Time.deltaTime;

            if(currentSprintReloadTime > sprintTime)
            {
                sprintMultiplicator = 1;
            }

            if(currentSprintReloadTime > sprintReloadTime)
            {
                useDash = false;
                currentSprintReloadTime = 0;
            }
        }
    }
}
