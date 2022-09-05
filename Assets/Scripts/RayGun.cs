using System;
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

    [SerializeField] private Scanner scanner;

    [SerializeField] private Transform paintRayContainer;
    [SerializeField] private PaintAngles paintAngles;
    [SerializeField] private float angleAdjustSensitivity;

    private float paintAngle;

    private GameObject[,] paintRays;

    private bool painting;

    public bool Scanning
    {
        get => scanner.Scanning;
        set
        {
            scanner.Scanning = value;
            painting = !Scanning && painting;
        }
    }

    public bool Painting
    {
        set
        {
            if (Scanning)
                return;

            painting = value;
            SetRaysActive(paintRays, painting);
        }
    }

    private void Awake()
    {
        SetupPainting();
        scanner.Setup(dotRenderer, rayDistance, dotColor, rayPrefab);
    }

    private void Update()
    {
        // Must set barrel light active first since Painting tries to deactivate itself every frame
        barrelLight.SetActive(painting || Scanning);
        Paint();
        scanner.Scan(Time.deltaTime);
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

    // Paint dots on the geometry in a scattered circle pattern
    private void Paint()
    {
        if (!painting)
            return;

        AdjustPaintRays();
        Painting = false;
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
