using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerMovement: MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float horizontalSpeed;
    [SerializeField] private float verticalJumpSpeed;
    [SerializeField] private float gravity = 9.81f;

    private Transform target;

    public float ForwardMotion { get; set; } = 0;
    public float SidewaysMotion { get; set; } = 0;
    private float UpwardMotion { get; set; } = 0;

    private Vector3 HorizontalVelocity =>
        (target.forward * ForwardMotion + target.right * SidewaysMotion).normalized * horizontalSpeed;
    private Vector3 VerticalVelocity => Vector3.up * UpwardMotion;
    private Vector3 Velocity => HorizontalVelocity + VerticalVelocity;
    private bool IsJumping => UpwardMotion > 0;


    private void Awake()
    {
        target = GetComponent<Transform>();
    }

    private void Update()
    {
        SetUpwardMotion(Time.deltaTime);
        ApplyMotion(Time.deltaTime, Velocity);
    }

    public void Jump()
    {
        if (!characterController.isGrounded)
            return;

        UpwardMotion = verticalJumpSpeed;
    }

    private void SetUpwardMotion(float deltaTime)
    {
        if (characterController.isGrounded && !IsJumping)
            UpwardMotion = 0;

        UpwardMotion -= gravity * deltaTime;
    }

    // Zero out motion after moving
    private void ApplyMotion(float deltaTime, Vector3 motion)
    {
        characterController.Move(deltaTime * motion);
        ForwardMotion = SidewaysMotion = 0;
    }
}