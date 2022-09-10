using System;
using UnityEngine;

public abstract class RayGunMode
{
    private PointRenderer pointRenderer;
    private float rayDistance;
    private LayerMask layerMask;

    public abstract void InitializeRays(GameObject rayPrefab);

    public void Setup(PointRenderer pointRenderer, float rayDistance,
        LayerMask layerMask, GameObject rayPrefab)
    {
        this.pointRenderer = pointRenderer;
        this.rayDistance = rayDistance;
        this.layerMask = layerMask;

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
        pointRenderer.CachePoint(hit.point);
    }

    public void SetRaysActive(Array rays, bool active)
    {
        foreach (GameObject ray in rays)
        {
            ray.SetActive(active);
        }
    }
}