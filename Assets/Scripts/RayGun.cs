using UnityEngine;

public class RayGun : MonoBehaviour
{
    [SerializeField] private Transform rayContainer;
    [SerializeField] private GameObject rayPrefab;
    [SerializeField] private int raysPerLayer;
    [SerializeField] private int numOfLayers;
    [SerializeField] private float angleFromCenter;

    private GameObject[,] paintRays;

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
        Debug.Log(paintRays.Length);
    }

    private void Update()
    {
        RandomlyOrientRays(paintRays);
    }

    public void Scan()
    {

    }

    // Paint dots on the geometry
    public void Paint()
    {
    }

    private void RandomlyOrientRays(GameObject[,] rays)
    {
        for (int i = 0; i < numOfLayers; i++)
        {
            float angleFromCenter = this.angleFromCenter * ((float)i + 1 / numOfLayers);
            float radianOffset = Random.Range(0, 2 * Mathf.PI);

            for (int j = 0; j < raysPerLayer; j++)
            {
                float radians = 2 * Mathf.PI * ((float)j / raysPerLayer);
                rays[i, j].transform.localEulerAngles = angleFromCenter *
                    new Vector3(Mathf.Sin(radians), Mathf.Cos(radians), 0);
            }
        }
    }
}
