using UnityEngine;

public class FirstPersonView : MonoBehaviour
{
    [SerializeField] private Transform firstPersonCamera;
    [SerializeField] private Transform player;

    [SerializeField] private Vector2 sensitivity = new Vector2(15, 15);

    [SerializeField] private float yMin = -80F;
    [SerializeField] private float yMax = 80F;

    // Y rotation must be cached since it's clamped;
    // X Rotation is unbounded, thus it doesn't appear as an instance variable
    private float rotationY;

    private float RotationX => firstPersonCamera.localEulerAngles.y + Input.GetAxisRaw("Mouse X") * sensitivity.x;
    private float RotationY
    {
        get
        {
            rotationY = Mathf.Clamp(rotationY + Input.GetAxisRaw("Mouse Y") * sensitivity.y, yMin, yMax);
            return -rotationY;
        }
    }

    private void Awake()
    {
        rotationY = 0;
    }

    private void Update()
    {
        SetCameraRotation(Vector3.right * RotationY);
        ApplyPlayerRotation(Vector3.up * RotationX);
    }

    // Set camera rotation (instead of applying a rotation) since it must be clamped
    private void SetCameraRotation(Vector3 rotation)
    {
        firstPersonCamera.localEulerAngles = rotation;
    }

    private void ApplyPlayerRotation(Vector3 rotation)
    {
        player.Rotate(rotation);
    }
}
