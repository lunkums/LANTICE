using UnityEngine;

public class PlayerMovement: MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float speed;

    private Transform target;

    public float ForwardMotion { get; set; } = 0;
    public float SidewaysMotion { get; set; } = 0;

    private Vector3 Velocity => (target.forward * ForwardMotion + target.right * SidewaysMotion).normalized * speed;

    private void Awake()
    {
        target = GetComponent<Transform>();
    }

    private void Update()
    {
        ApplyMotion(Time.deltaTime, Velocity);
    }

    // Zero out motion after moving
    private void ApplyMotion(float deltaTime, Vector3 motion)
    {
        characterController.Move(deltaTime * motion);
        ForwardMotion = SidewaysMotion = 0;
    }
}