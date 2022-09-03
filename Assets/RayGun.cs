using UnityEngine;

public class RayGun : MonoBehaviour
{
    [SerializeField] private Transform rayContainer;
    [SerializeField] private GameObject rayPrefab;
    [SerializeField] private int numOfRays;
    [SerializeField] private float angleFromCenter;

    private void Awake()
    {
        for (int i = 0; i < numOfRays; i++)
        {
            GameObject ray = Instantiate(rayPrefab, rayContainer);
            float radians = 2 * Mathf.PI * ((float)i / numOfRays);
            ray.transform.rotation = Quaternion.Euler(angleFromCenter * new Vector3(Mathf.Sin(radians), Mathf.Cos(radians), 0));
        }
    }

    public void Scan()
    {

    }

    // Paint dots on the geometry
    public void Paint()
    {
    }
}
