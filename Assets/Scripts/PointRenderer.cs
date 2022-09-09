using Lunkums.Util;
using UnityEngine;
using UnityEngine.VFX;

public class PointRenderer : MonoBehaviour
{
    private const int TEXTURE_2D_MAX_HEIGHT = 16384;

    private VisualEffect currentEffect;
    private Texture2D texture2d;
    private Color[] positions;
    private int particleCount;

    private const string POSITIONS_TEXTURE_NAME = "Positions";
    private const string CAPACITY_PARAM_NAME = "Capacity";

    [SerializeField] private VisualEffect effectPrefab;
    [SerializeField] private Transform effectContainer;

    private void Awake()
    {
        positions = new Color[TEXTURE_2D_MAX_HEIGHT];
    }

    private void Start()
    {
        CreateNewEffect();
        ApplyPositions();
    }

    private void FixedUpdate()
    {
        ApplyPositions();
    }

    public void CreatePoint(Vector3 position)
    {
        positions[particleCount] = new Color(position.x, position.y, position.z);
        particleCount++;

        if (particleCount >= TEXTURE_2D_MAX_HEIGHT)
        {
            CreateNewEffect();
        }
    }

    public void ClearAllPoints()
    {
        CreateNewEffect();
    }

    private void ApplyPositions()
    {
        texture2d.SetPixels(positions);
        texture2d.Apply();

        currentEffect.SetTexture(POSITIONS_TEXTURE_NAME, texture2d);
        currentEffect.Reinit();
    }

    private void CreateNewEffect()
    {
        currentEffect = Instantiate(effectPrefab, effectContainer);
        currentEffect.SetUInt(CAPACITY_PARAM_NAME, (uint)TEXTURE_2D_MAX_HEIGHT);
        texture2d = new Texture2D(TEXTURE_2D_MAX_HEIGHT, 1, TextureFormat.RGBAFloat, false);
        positions.Default(new Color(0, 0, 0, 0));
        particleCount = 0;
    }
}
