using UnityEngine;

public class FirstPersonView : MonoBehaviour
{
    [SerializeField] private Transform firstPersonView;
    [SerializeField] private Transform player;

    [SerializeField] private Vector2 sensitivity = new Vector2(15, 15);
    [SerializeField] private int minPitch = -80;
    [SerializeField] private int maxPitch = 80;

    private float yaw;
    private float pitch;

    private void Awake()
    {
        yaw = 0;
        pitch = 0;
    }

    private void Update()
    {
        AdjustPitch(Time.deltaTime);
        AdjustYaw(Time.deltaTime);

        SetCameraXRotation(Vector3.right * pitch);
        SetPlayerYRotation(Vector3.up * yaw);
    }

    private void AdjustPitch(float deltaTime)
    {
        pitch -= GetAdjustedSensitivity(deltaTime).y * Input.GetAxisRaw("Mouse Y") * deltaTime;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
    }

    private void AdjustYaw(float deltaTime)
    {
        yaw += GetAdjustedSensitivity(deltaTime).x * Input.GetAxisRaw("Mouse X") * deltaTime;
    }

    // Adjusted sensitivity allows for easier control over the base sensitivity while still allowing for the effects of
    // a timescale of 0.
    private Vector2 GetAdjustedSensitivity(float deltaTime)
    {
        return deltaTime.IsZero() ? Vector2.zero : sensitivity / deltaTime;
    }

    private void SetCameraXRotation(Vector3 rotation)
    {
        firstPersonView.localEulerAngles = rotation;
    }

    private void SetPlayerYRotation(Vector3 rotation)
    {
        player.localEulerAngles = rotation;
    }
}