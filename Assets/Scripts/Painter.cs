using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Painter : RayGunMode
{
    [SerializeField] private Transform rayContainer;
    [SerializeField] private int raysPerLayer;
    [SerializeField] private int numOfLayers;
    [SerializeField] private Angles angles;
    [SerializeField] private float angleAdjustSensitivity;

    private float paintAngle;
    private GameObject[,] paintRays;

    private bool painting;

    public bool Painting
    {
        get => painting;
        set
        {
            painting = value;
            SetRaysActive(paintRays, painting);
        }
    }

    public override void InitializeRays(GameObject rayPrefab)
    {
        paintRays = new GameObject[raysPerLayer, numOfLayers];
        paintAngle = angles.initial;

        for (int i = 0; i < numOfLayers; i++)
        {
            for (int j = 0; j < raysPerLayer; j++)
            {
                paintRays[i, j] = GameObject.Instantiate(rayPrefab, rayContainer);
            }
        }

        Painting = false;
    }

    public void AdjustAngle(float scrollDelta)
    {
        // Return if not painting so rays don't awkwardly jump to the new angle after adjusting while painting is off
        // Could also call AdjustRays() right before enabling them, similar to scanner
        if (!painting)
            return;

        paintAngle = Mathf.Clamp(paintAngle + scrollDelta * angleAdjustSensitivity, angles.min, angles.max);
    }

    // Paint dots on the geometry in a scattered circle pattern
    public void Paint()
    {
        if (!painting)
            return;

        AdjustRays();
        Painting = false;
    }

    // Randomizes the orientation of all paint rays, resizes them to the distance from the surface they hit, and paints
    // a dot for the first ray in each ray layer
    private void AdjustRays()
    {
        RaycastHit hit = new RaycastHit();

        for (int i = 0; i < numOfLayers; i++)
        {
            float angleFromCenter = Random.Range(
                paintAngle * (i / (float)numOfLayers),
                paintAngle * ((i + 1) / (float)numOfLayers));
            float radianOffset = Random.Range(0, 2 * Mathf.PI);

            for (int j = 0; j < raysPerLayer; j++)
            {
                float radians = 2 * Mathf.PI * (j / (float)raysPerLayer) + radianOffset;
                if (AdjustRayFromRaycast(paintRays[i, j].transform, angleFromCenter, radians, ref hit))
                    CreateDotFromRaycast(hit);
            }
        }
    }

    // Tries to adjust the paint ray from a raycast, returning whether the raycast hit was successful
    private bool AdjustRayFromRaycast(Transform ray, float angleFromCenter, float radians, ref RaycastHit hit)
    {
        bool successfulHit;

        // Orient the ray
        ray.localEulerAngles = angleFromCenter *
            new Vector3(Mathf.Sin(radians), Mathf.Cos(radians), 0);

        successfulHit = Raycast(ray.position, ray.forward, out hit);
        ResizeRay(ray, hit.distance);

        return successfulHit;
    }

    // Paint angles are measured from center line to edge of paint volume (half of the "point" angle of a cone)
    [Serializable]
    private struct Angles
    {
        public float min;
        public float max;
        public float initial;
    }
}
