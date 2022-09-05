using System;
using UnityEngine;

public abstract class RayGunMode : MonoBehaviour
{
    private DotRenderer dotRenderer;
    private float rayDistance;
    private Color dotColor;

    public float RayDistance => rayDistance;

    public abstract void InitializeRays(GameObject rayPrefab);

    public void Setup(DotRenderer dotRenderer, float rayDistance, Color dotColor, GameObject rayPrefab)
    {
        this.dotRenderer = dotRenderer;
        this.rayDistance = rayDistance;
        this.dotColor = dotColor;

        InitializeRays(rayPrefab);
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