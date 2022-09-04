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
        SetCameraXRotation(Vector3.right * pitch);
        SetPlayerYRotation(Vector3.up * yaw);
    }

    public void AdjustView(Vector2 mouseInput)
    {
        AdjustPitch(mouseInput.y);
        AdjustYaw(mouseInput.x);
    }

    private void AdjustPitch(float deltaY)
    {
        pitch -= sensitivity.y * deltaY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
    }

    private void AdjustYaw(float deltaX)
    {
        yaw += sensitivity.x * deltaX;
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