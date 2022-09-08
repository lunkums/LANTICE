using System;
using UnityEngine;

public abstract class RayGunMode : MonoBehaviour
{
    private DotRenderer dotRenderer;
    private float rayDistance;
    private LayerMask layerMask;
    private Color dotColor;

    public abstract void InitializeRays(GameObject rayPrefab);

    public void Setup(DotRenderer dotRenderer, float rayDistance,
        LayerMask layerMask, Color dotColor, GameObject rayPrefab)
    {
        this.dotRenderer = dotRenderer;
        this.rayDistance = rayDistance;
        this.layerMask = layerMask;
        this.dotColor = dotColor;

        InitializeRays(rayPrefab);
    }

    // Attempt a raycast, defaulting the hit distance to 0 if it fails
    public bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo)
    {
        if (!Physics.Raycast(origin, direction, out hitInfo, rayDistance, layerMask))
        {
            hitInfo.distance = 0;
            return false;
        }
        return true;
    }

    public void ResizeRay(Transform ray, float length)
    {
        ray.localScale = new Vector3(1, 1, length);
    }

    public void CreateDotFromRaycast(RaycastHit hit)
    {
        dotRenderer.CreateDot(
            hit.point,
            Quaternion.FromToRotation(Vector3.forward, hit.transform.forward),
            dotColor);
    }

    public void SetRaysActive(Array rays, bool active)
    {
        foreach (GameObject ray in rays)
        {
            ray.SetActive(active);
        }
    }
}