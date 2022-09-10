using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Scanner : RayGunMode
{
    [SerializeField] private Transform rayContainer;
    [SerializeField] private int numOfRays;
    [SerializeField] private float verticalScanAngle;
    [SerializeField] private float horizontalScanAngle;
    [SerializeField] private float scanRate;

    private GameObject[] rays;
    private float scanAngle;

    private bool scanning;

    public bool Scanning
    {
        get => scanning;
        set
        {
            if (scanAngle < verticalScanAngle)
                return;

            scanning = value;
            scanAngle = scanning ? -verticalScanAngle : verticalScanAngle;
            SetRaysActive(rays, scanning);
        }
    }

    public override void InitializeRays(GameObject rayPrefab)
    {
        rays = new GameObject[numOfRays];
        scanAngle = verticalScanAngle;

        for (int i = 0; i < numOfRays; i++)
        {
            rays[i] = GameObject.Instantiate(rayPrefab, rayContainer);
        }

        Scanning = false;
    }

    // Scan the geometry, drawing horizontal lines down the geometry
    public void Scan(float deltaTime)
    {
        if (!scanning)
            return;

        scanAngle += scanRate * deltaTime;
        AdjustRays();

        Scanning = false;
    }

    private void AdjustRays()
    {
        RaycastHit hit = new RaycastHit();

        for (int i = 0; i < numOfRays; i++)
        {
            if (AdjustRayFromRaycast(
                rays[i].transform,
                horizontalScanAngle,
                scanAngle,
                Mathf.PI * Random.Range(i / (float)numOfRays, i + 1 / (float)numOfRays),
                ref hit))
            {
                CreateDotFromRaycast(hit);
            }
        }
    }

    // Tries to adjust the scan ray from a raycast, returning whether the raycast hit was successful
    private bool AdjustRayFromRaycast(Transform ray, float horizontalAngle, float verticalAngle,
        float horizontalRadians, ref RaycastHit hit)
    {
        bool successfulHit;

        // Orient the ray
        ray.localEulerAngles = new Vector3(verticalAngle, Mathf.Cos(horizontalRadians) * horizontalAngle, 0);

        successfulHit = Raycast(ray.position, ray.forward, out hit);
        ResizeRay(ray, hit.distance);

        return successfulHit;
    }

    public new void SetRaysActive(Array rays, bool active)
    {
        // Adjust rays before enabling them so they don't appear to jump to the top of the screen
        if (active)
            AdjustRays();

        base.SetRaysActive(rays, active);
    }
}
