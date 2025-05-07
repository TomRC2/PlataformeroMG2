using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float walkSpeed = 5f;
    public float runSpeed = 9f;

    [Header("Salto y gravedad")]
    public float jumpHeight = 2f;
    public float gravity = -20f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundMask;

    [Header("Cámara")]
    public Transform cameraPivot;

    private CharacterController controller;
    private Vector3 velocity;
    private Vector3 externalVelocity = Vector3.zero;
    private bool isGrounded;

    private float originalWalkSpeed;
    private float originalRunSpeed;
    private float originalJumpHeight;

    private Vector3 respawnPoint;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        StoreOriginalStats();
        respawnPoint = transform.position;
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 forward = cameraPivot.forward;
        Vector3 right = cameraPivot.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * z + right * x;
        moveDirection.Normalize();

        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        Vector3 move = moveDirection * speed;

        if (moveDirection != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), 0.15f);

        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;

        Vector3 finalVelocity = move + new Vector3(0, velocity.y, 0) + externalVelocity;
        controller.Move(finalVelocity * Time.deltaTime);

        externalVelocity = Vector3.Lerp(externalVelocity, Vector3.zero, Time.deltaTime * 2f);

        if (transform.position.y < -10f)
        {
            Respawn();
        }
    }

    public void ExternalLaunch(Vector3 force)
    {
        externalVelocity = force;
    }

    void StoreOriginalStats()
    {
        originalWalkSpeed = walkSpeed;
        originalRunSpeed = runSpeed;
        originalJumpHeight = jumpHeight;
    }

    public void ApplyJumpBoost(float multiplier, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(JumpBoostRoutine(multiplier, duration));
    }

    public void ApplySpeedBoost(float multiplier, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(SpeedBoostRoutine(multiplier, duration));
    }

    IEnumerator JumpBoostRoutine(float multiplier, float duration)
    {
        jumpHeight = originalJumpHeight * multiplier;
        yield return new WaitForSeconds(duration);
        jumpHeight = originalJumpHeight;
    }

    IEnumerator SpeedBoostRoutine(float multiplier, float duration)
    {
        walkSpeed = originalWalkSpeed * multiplier;
        runSpeed = originalRunSpeed * multiplier;
        yield return new WaitForSeconds(duration);
        walkSpeed = originalWalkSpeed;
        runSpeed = originalRunSpeed;
    }

    public void SetCheckpoint(Vector3 newCheckpoint)
    {
        respawnPoint = newCheckpoint;
    }
    public void Respawn()
    {
        controller.enabled = false;
        transform.position = respawnPoint;
        controller.enabled = true;

        velocity = Vector3.zero;
    }
}
