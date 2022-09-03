using UnityEngine;

public class LineBaker : MonoBehaviour
{
    [SerializeField] private Material material;

    private void Awake()
    {
        BakeLine(gameObject, material);
    }

    private static void BakeLine(GameObject lineObj, Material material)
    {
        LineRenderer lineRenderer = lineObj.GetComponent<LineRenderer>();
        MeshFilter meshFilter = lineObj.AddComponent<MeshFilter>();
        Mesh mesh = new Mesh();

        lineRenderer.BakeMesh(mesh);
        meshFilter.sharedMesh = mesh;

        MeshRenderer meshRenderer = lineObj.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = material;

        Destroy(lineRenderer);
    }
}
