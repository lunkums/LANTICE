using System;
using UnityEngine;

[Obsolete("Bottlenecked by the compute buffer size; use VFX graph instead.")]
public class DotRenderer : MonoBehaviour
{
    [SerializeField] private int population;
    [SerializeField] private Material material;
    [SerializeField] private Vector3 dotScale;

    private MeshProperties[] meshProperties;
    private ComputeBuffer meshPropertiesBuffer;
    private ComputeBuffer argsBuffer;

    private Mesh mesh;
    private Bounds bounds;
    private int activeMeshInstances;

    private void Start()
    {
        Setup();
    }

    private void Update()
    {
        Graphics.DrawMeshInstancedIndirect(mesh, 0, material, bounds, argsBuffer);
    }

    private void OnDisable()
    {
        // Release gracefully.
        if (meshPropertiesBuffer != null)
        {
            meshPropertiesBuffer.Release();
        }
        meshPropertiesBuffer = null;

        if (argsBuffer != null)
        {
            argsBuffer.Release();
        }
        argsBuffer = null;
    }

    public void CreateDot(Vector3 position, Quaternion rotation, Color color)
    {
        meshProperties[activeMeshInstances] = new MeshProperties
        {
            mat = Matrix4x4.TRS(position, rotation, dotScale),
            color = color
        };
        meshPropertiesBuffer.SetData(meshProperties, activeMeshInstances, activeMeshInstances, 1);
        activeMeshInstances = (activeMeshInstances + 1) % population;
    }

    // Should be used carefully as a debug action
    public void Clear()
    {
        meshPropertiesBuffer.Release();
        meshPropertiesBuffer = new ComputeBuffer(population, MeshProperties.Size());
        material.SetBuffer("_Properties", meshPropertiesBuffer);
    }

    private void Setup()
    {
        mesh = CreateQuad();
        // Boundary surrounding the meshes we will be drawing.  Used for occlusion.
        bounds = new Bounds(transform.position, Vector3.one * 1000);
        meshProperties = InitializeBuffers();
        activeMeshInstances = 0;
    }

    private MeshProperties[] InitializeBuffers()
    {
        // Argument buffer used by DrawMeshInstancedIndirect.
        uint[] args = new uint[5] { 0, 0, 0, 0, 0 };
        // Arguments for drawing mesh.
        // 0 == number of triangle indices, 1 == population, others are only relevant if drawing submeshes.
        args[0] = (uint)mesh.GetIndexCount(0);
        args[1] = (uint)population;
        args[2] = (uint)mesh.GetIndexStart(0);
        args[3] = (uint)mesh.GetBaseVertex(0);
        argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
        argsBuffer.SetData(args);

        // Initialize buffer with the given population.
        MeshProperties[] properties = new MeshProperties[population];

        meshPropertiesBuffer = new ComputeBuffer(population, MeshProperties.Size());
        meshPropertiesBuffer.SetData(properties);
        material.SetBuffer("_Properties", meshPropertiesBuffer);

        return properties;
    }

    private Mesh CreateQuad(float width = 1f, float height = 1f)
    {
        // Create a quad mesh.
        var mesh = new Mesh();

        float w = width * .5f;
        float h = height * .5f;
        var vertices = new Vector3[4] {
            new Vector3(-w, -h, 0),
            new Vector3(w, -h, 0),
            new Vector3(-w, h, 0),
            new Vector3(w, h, 0)
        };

        var tris = new int[6] {
            // lower left tri.
            0, 2, 1,
            // lower right tri
            2, 3, 1
        };

        var normals = new Vector3[4] {
            Vector3.back,
            Vector3.back,
            Vector3.back,
            Vector3.back,
        };

        var uv = new Vector2[4] {
            Vector2.zero,
            Vector2.right,
            Vector2.up,
            Vector2.one
        };

        mesh.vertices = vertices;
        mesh.triangles = tris;
        mesh.normals = normals;
        mesh.uv = uv;

        return mesh;
    }

    // Mesh Properties struct to be read from the GPU.
    // Size() is a convenience function which returns the stride of the struct.
    private struct MeshProperties
    {
        public Matrix4x4 mat;
        public Vector4 color;

        public static int Size()
        {
            return
                sizeof(float) * 4 * 4 + // matrix;
                sizeof(float) * 4;      // color;
        }
    }
}