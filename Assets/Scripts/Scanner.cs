using UnityEngine;
using Random = UnityEngine.Random;

public class Scanner : RayGunMode
{
    [SerializeField] private Transform scanRayContainer;
    [SerializeField] private int numOfScanRays;
    [SerializeField] private float verticalScanAngle;
    [SerializeField] private float horizontalScanAngle;
    [SerializeField] private float scanRate;

    private float scanAngleRadians;
    // Either 1 or 0 - used to randomly sample half of all scan rays while scanning
    private int scanRayPass;
    private GameObject[] scanRays;

    private bool scanning;

    public bool Scanning
    {
        get => scanning;
        set
        {
            if (scanAngleRadians > 0)
                return;

            scanning = value;
            SetRaysActive(scanRays, scanning);
            scanAngleRadians = scanning ? Mathf.PI : -1;
        }
    }

    public void Setup(DotRenderer dotRenderer, float rayDistance, Color dotColor, GameObject rayPrefab)
    {
        Setup(dotRenderer, rayDistance, dotColor);

        scanRays = new GameObject[numOfScanRays];
        scanAngleRadians = -1;
        scanRayPass = 0;

        for (int i = 0; i < numOfScanRays; i++)
        {
            scanRays[i] = Instantiate(rayPrefab, scanRayContainer);
        }

        Scanning = false;
    }

    // Scan the geometry, drawing horizontal lines down the geometry
    public void Scan(float deltaTime)
    {
        if (!scanning)
            return;

        scanAngleRadians -= scanRate * deltaTime;
        AdjustScanRays();

        Scanning = false;
    }

    private void AdjustScanRays()
    {
        RaycastHit hit = new RaycastHit();
        scanRayPass = (scanRayPass + 1) % 2;

        for (int i = 0; i < numOfScanRays; i++)
        {
            if (AdjustScanRayFromRaycast(
                scanRays[i].transform,
                horizontalScanAngle,
                verticalScanAngle,
                Mathf.PI * Random.Range(i / (float)numOfScanRays, i + 1 / (float)numOfScanRays),
                scanAngleRadians,
                ref hit)
                && i % 2 == scanRayPass)
            {
                CreateDotFromRaycast(hit);
            }
        }
    }

    // Tries to adjust the scan ray from a raycast, returning whether the raycast hit was successful
    private bool AdjustScanRayFromRaycast(Transform ray, float horizontalAngle, float verticalAngle,
        float horizontalRadians, float verticalRadians, ref RaycastHit hit)
    {
        // Orient the ray
        ray.localEulerAngles = new Vector3(
            Mathf.Cos(verticalRadians) * verticalAngle,
            Mathf.Cos(horizontalRadians) * horizontalAngle, 0);

        if (!Physics.Raycast(ray.position, ray.forward, out hit, RayDistance))
        {
            ResizeRay(ray, 0);
            return false;
        }

        ResizeRay(ray, hit.distance);
        return true;
    }
}
