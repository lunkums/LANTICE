using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RayGun : MonoBehaviour
{
    [SerializeField] private DotRenderer dotRenderer;
    [SerializeField] private Color dotColor = Color.white;

    [SerializeField] private GameObject rayPrefab;
    [SerializeField] private float rayDistance;
    [SerializeField] private int raysPerLayer;
    [SerializeField] private int numOfLayers;

    [SerializeField] private GameObject barrelLight;

    [SerializeField] private Transform paintRayContainer;
    [SerializeField] private PaintAngles paintAngles;
    [SerializeField] private float angleAdjustSensitivity;

    // Scan speed is how fast the scan goes, scan poll rate is how often dots are drawn while scanning;
    [SerializeField] private Transform scanRayContainer;
    [SerializeField] private int numOfScanRays;
    [SerializeField] private float verticalScanAngle;
    [SerializeField] private float horizontalScanAngle;
    [SerializeField] private float scanRate;

    private float paintAngle;
    private float scanAngle;

    private GameObject[,] paintRays;
    private GameObject[] scanRays;

    private bool scanning;
    private bool painting;

    public bool Scanning
    {
        set
        {
            if (scanAngle > 0)
                return;

            if (scanning = value)
            {
                scanAngle = Mathf.PI;
                Painting = false;
            }
            SetRaysActive(scanRays, scanning);
        }
    }

    public bool Painting
    {
        set
        {
            if (scanning)
                return;

            painting = value;
            SetRaysActive(paintRays, painting);
        }
    }

    private void Awake()
    {
        SetupPainting();
        SetupScanning();
    }

    private void Update()
    {
        // Must set barrel light active first since Painting tries to deactivate itself every frame
        barrelLight.SetActive(painting || scanning);
        Paint();
        Scan(Time.deltaTime);
    }

    public void AdjustPaintAngle(float scrollDelta)
    {
        paintAngle = Mathf.Clamp(paintAngle + scrollDelta * angleAdjustSensitivity, paintAngles.min, paintAngles.max);
    }

    private void SetupPainting()
    {
        paintRays = new GameObject[raysPerLayer, numOfLayers];
        paintAngle = paintAngles.initial;

        for (int i = 0; i < numOfLayers; i++)
        {
            for (int j = 0; j < raysPerLayer; j++)
            {
                paintRays[i, j] = Instantiate(rayPrefab, paintRayContainer);
            }
        }

        Painting = false;
    }

    private void SetupScanning()
    {
        scanRays = new GameObject[numOfScanRays];
        scanAngle = -1;

        for (int i = 0; i < numOfScanRays; i++)
        {
            scanRays[i] = Instantiate(rayPrefab, scanRayContainer);
        }

        Scanning = false;
    }

    // Paint dots on the geometry in a scattered circle pattern
    private void Paint()
    {
        if (!painting)
            return;

        AdjustPaintRays();
        Painting = false;
    }

    // Scan the geometry, drawing horizontal lines down the geometry
    private void Scan(float deltaTime)
    {
        if (!scanning)
            return;

        scanAngle -= scanRate * deltaTime;
        AdjustScanRays();

        Scanning = false;
    }

    // Randomizes the orientation of all paint rays, resizes them to the distance from the surface they hit, and paints
    // a dot for the first ray in each ray layer
    private void AdjustPaintRays()
    {
        for (int i = 0; i < numOfLayers; i++)
        {
            float angleFromCenter = Random.Range(
                paintAngle * (i / (float)numOfLayers),
                paintAngle * ((i + 1) / (float)numOfLayers));
            float radianOffset = Random.Range(0, 2 * Mathf.PI);
            RaycastHit hit = new RaycastHit();

            if (AdjustPaintRayFromRaycast(paintRays[i, 0].transform, angleFromCenter, radianOffset, ref hit))
                CreateDotFromRaycast(hit);

            for (int j = 1; j < raysPerLayer; j++)
            {
                float radians = 2 * Mathf.PI * (j / (float)raysPerLayer) + radianOffset;
                AdjustPaintRayFromRaycast(paintRays[i, j].transform, angleFromCenter, radians, ref hit);
            }
        }
    }

    private void AdjustScanRays()
    {
        RaycastHit hit = new RaycastHit();

        for (int i = 0; i < numOfScanRays; i++)
        {
            if (AdjustScanRayFromRaycast(
                scanRays[i].transform, horizontalScanAngle,
                verticalScanAngle,
                Mathf.PI * ((float)i / numOfScanRays - 1),
                scanAngle,
                ref hit))
            {
                CreateDotFromRaycast(hit);
            }
        }
    }

    // Tries to adjust the paint ray from a raycast, returning whether the raycast hit was successful
    private bool AdjustPaintRayFromRaycast(Transform ray, float angleFromCenter, float radians, ref RaycastHit hit)
    {
        // Orient the ray
        ray.localEulerAngles = angleFromCenter *
            new Vector3(Mathf.Sin(radians), Mathf.Cos(radians), 0);

        if (!Physics.Raycast(ray.position, ray.forward, out hit, rayDistance))
        {
            ResizeRay(ray, 0);
            return false;
        }

        ResizeRay(ray, hit.distance);
        return true;
    }

    // Tries to adjust the scan ray from a raycast, returning whether the raycast hit was successful
    private bool AdjustScanRayFromRaycast(Transform ray, float horizontalAngle, float verticalAngle,
        float horizontalRadians, float verticalRadians, ref RaycastHit hit)
    {
        // Orient the ray
        ray.localEulerAngles = new Vector3(
            Mathf.Cos(verticalRadians) * verticalAngle,
            Mathf.Cos(horizontalRadians) * horizontalAngle, 0);

        if (!Physics.Raycast(ray.position, ray.forward, out hit, rayDistance))
        {
            ResizeRay(ray, 0);
            return false;
        }

        ResizeRay(ray, hit.distance);
        return true;
    }

    private void ResizeRay(Transform ray, float length)
    {
        ray.localScale = new Vector3(1, 1, length);
    }

    private void CreateDotFromRaycast(RaycastHit hit)
    {
        dotRenderer.CreateDot(
            hit.point, 
            Quaternion.FromToRotation(Vector3.forward, hit.transform.forward),
            dotColor);
    }

    private void SetRaysActive(Array rays, bool active)
    {
        foreach (GameObject ray in rays)
        {
            ray.SetActive(active);
        }
    }

    // Paint angles are measured from center line to edge of paint volume (half of the "point" angle of a cone)
    [Serializable]
    private struct PaintAngles
    {
        public float min;
        public float max;
        public float initial;
    }
}
