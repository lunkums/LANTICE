using UnityEngine;

public class PointAim : MonoBehaviour
{
    [SerializeField] private Transform pointer;
    [SerializeField] private Transform rayOrigin;

    [SerializeField] private bool smooth;
    [SerializeField][Range(0.01f, 1)] private float smoothRate = 0.5f;

    private float AdjustedSmoothRate => smoothRate / Time.fixedDeltaTime;

    private void FixedUpdate()
    {
        Aim(Time.fixedDeltaTime, pointer);
    }

    private void Aim(float deltaTime, Transform transform)
    {
        if (!Physics.Raycast(rayOrigin.position, rayOrigin.forward, out RaycastHit hit))
            return;

        if (smooth)
        {
            Vector3 relativePos = hit.point - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(relativePos);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, AdjustedSmoothRate * deltaTime);
        }
        else
        {
            transform.LookAt(hit.point);
        }
    }
}