using UnityEngine;
using UnityEngine.VFX;

public class PointRenderer : MonoBehaviour
{
    // Value was chosen for max size of Texture2D
    private const int CAPACITY = 16384;

    [SerializeField] private Transform effectsContainer;
    [SerializeField] private VisualEffect visualEffectPrefab;

    [SerializeField] private string spawnEventName;
    [SerializeField] private string texturePropertyName;

    private VisualEffect currentEffect;
    private int particleCount;
    private Vector3[] positions;

    private void Awake()
    {
        SetNewEffect();
    }

    private void Update()
    {
        // Create texture
        var texture = new Texture2D(CAPACITY, 1, TextureFormat.RFloat, false)
        {
            filterMode = FilterMode.Point
        };

        // Set all of your particle positions in the texture
        var colors = new Color[CAPACITY];

        // Begin do this on every frame
        for (int i = 0; i < CAPACITY; i++)
        {
            colors[i] = new Color(positions[i].x, positions[i].y, positions[i].z, 0);
        }

        texture.SetPixels(colors);
        texture.Apply();
        // End do this on every frame
        currentEffect.SetTexture(texturePropertyName, texture);
    }

    public void CreatePoint(Vector3 position)
    {
        if (particleCount >= CAPACITY)
        {
            SetNewEffect();
        }

        positions[particleCount] = position;
        particleCount++;
    }

    public void ClearAllPoints()
    {
        foreach (Transform child in effectsContainer)
        {
            Destroy(child.gameObject);
        }

        SetNewEffect();
    }

    private void SetNewEffect()
    {
        currentEffect = Instantiate(visualEffectPrefab, effectsContainer);
        positions = new Vector3[CAPACITY];
        currentEffect.SendEvent(spawnEventName);
        particleCount = 0;
    }
}
