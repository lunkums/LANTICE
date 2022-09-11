using Lunkums.Util;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PointRenderer : MonoBehaviour
{
    private const int TEXTURE_2D_MAX_HEIGHT = 16384;

    private const string POSITIONS_TEXTURE_NAME = "Positions";
    private const string CAPACITY_PARAM_NAME = "Capacity";
    private const string REFERENCE_POS_PARAM_NAME = "ReferencePosition";

    [SerializeField] private VisualEffect effectPrefab;
    [SerializeField] private Transform effectContainer;

    private VisualEffect currentEffect;
    private Queue<VisualEffect> effects;
    private Texture2D texture2d;
    private Color[] points;
    private int particleCount;

    private void Awake()
    {
        points = new Color[TEXTURE_2D_MAX_HEIGHT];
        effects = new Queue<VisualEffect>();
    }

    private void Start()
    {
        CreateNewEffect();
        ApplyPoints();
    }

    private void FixedUpdate()
    {
        ApplyPoints();
    }

    public void CachePoint(Vector3 position)
    {
        points[particleCount] = new Color(position.x, position.y, position.z);
        particleCount++;

        if (particleCount >= TEXTURE_2D_MAX_HEIGHT)
        {
            CreateNewEffect();
        }
    }

    public void ClearAllPoints()
    {
        while (effects.Count > 0)
        {
            Destroy(effects.Dequeue().gameObject);
        }
        CreateNewEffect();
    }

    public void SetReferencePosition(Vector3 position)
    {
        foreach(VisualEffect effect in effects)
        {
            effect.SetVector3(REFERENCE_POS_PARAM_NAME, position);
        }
    }

    // Apply a cached array of points (positions) to a texture 2D to be loaded into the VFX graph
    private void ApplyPoints()
    {
        // Update the texture
        texture2d.SetPixels(points);
        texture2d.Apply();
        // Update the current effect with the updated texture
        currentEffect.SetTexture(POSITIONS_TEXTURE_NAME, texture2d);
        currentEffect.Reinit();
    }

    private void CreateNewEffect()
    {
        currentEffect = Instantiate(effectPrefab, effectContainer);
        currentEffect.SetUInt(CAPACITY_PARAM_NAME, TEXTURE_2D_MAX_HEIGHT);
        effects.Enqueue(currentEffect);
        texture2d = new Texture2D(TEXTURE_2D_MAX_HEIGHT, 1, TextureFormat.RGBAFloat, false);
        points.Default(new Color(0, 0, 0, 0));
        particleCount = 0;
    }
}
