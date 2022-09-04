using UnityEngine;

public class RayGun : MonoBehaviour
{
    [SerializeField] private Transform rayContainer;
    [SerializeField] private GameObject rayPrefab;
    [SerializeField] private int raysPerLayer;
    [SerializeField] private int numOfLayers;
    [SerializeField] private float angleFromCenter;

    private GameObject[,] paintRays;

    private bool scanning;
    private bool painting;

    public bool Scanning
    {
        set
        {
            if (scanning = value)
            {
                Painting = false;
            }
        }
    }

    public bool Painting
    {
        set
        {
            if (painting = value)
            {
                Scanning = false;
            }
            SetActiveRays(paintRays, painting);
        }
    }

    private void Awake()
    {
        paintRays = new GameObject[raysPerLayer, numOfLayers];
        for (int i = 0; i < numOfLayers; i++)
        {
            for (int j = 0; j < raysPerLayer; j++)
            {
                paintRays[i, j] = Instantiate(rayPrefab, rayContainer);
            }
        }

        Scanning = Painting = false;
    }

    private void Update()
    {
        Paint();
        Scan();
    }

    public void Scan()
    {
        if (!scanning)
            return;

        Scanning = false;
    }

    // Paint dots on the geometry
    public void Paint()
    {
        if (!painting)
            return;

        RandomlyOrientRays(paintRays);
        Painting = false;
    }

    private void RandomlyOrientRays(GameObject[,] rays)
    {
        for (int i = 0; i < numOfLayers; i++)
        {
            float angleFromCenter = this.angleFromCenter * ((i + 1) / (float)numOfLayers);
            float radianOffset = Random.Range(0, 2 * Mathf.PI);

            for (int j = 0; j < raysPerLayer; j++)
            {
                float radians = 2 * Mathf.PI * (j / (float)raysPerLayer) + radianOffset;
                rays[i, j].transform.localEulerAngles = angleFromCenter *
                    new Vector3(Mathf.Sin(radians), Mathf.Cos(radians), 0);
            }
        }
    }

    private void SetActiveRays(GameObject[,] rays, bool active)
    {
        foreach (GameObject ray in rays)
        {
            ray.SetActive(active);
        }
    }
}
