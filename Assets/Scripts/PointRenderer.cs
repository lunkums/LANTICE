//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.VFX;

//public class PointRenderer : MonoBehaviour
//{
//    // Value was chosen for max size of Texture2D
//    private const int CAPACITY = 16384;

//    [SerializeField] private Transform effectsContainer;
//    [SerializeField] private VisualEffect visualEffectPrefab;

//    [SerializeField] private string texturePropertyName;

//    private VisualEffect currentEffect;
//    private int particleCount;
//    private Color[] positions;
//    private Texture2D texture2d;

//    private void Awake()
//    {
//        SetNewEffect();
//    }

//    private void Update()
//    {
//        // apply to texture
//        texture2d.SetPixels(positions);
//        texture2d.Apply();

//        // apply to VFX
//        currentEffect.SetTexture(texturePropertyName, texture2d);
//        currentEffect.Reinit();
//    }

//    public void CreatePoint(Vector3 position)
//    {
//        if (particleCount >= CAPACITY)
//        {
//            SetNewEffect();
//        }

//        positions[particleCount] = new Color(position.x, position.y, position.z);
//        particleCount++;
//    }

//    public void ClearAllPoints()
//    {
//        foreach (Transform child in effectsContainer)
//        {
//            Destroy(child.gameObject);
//        }

//        SetNewEffect();
//    }

//    private void SetNewEffect()
//    {
//        currentEffect = Instantiate(visualEffectPrefab, effectsContainer);
//        positions = new Color[CAPACITY];
//        texture2d = new Texture2D(CAPACITY, 1, TextureFormat.RGBAFloat, false)
//        {
//            filterMode = FilterMode.Point
//        };
//        currentEffect.SetTexture(texturePropertyName, texture2d);
//        particleCount = 0;
//    }
//}


using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PointRenderer : MonoBehaviour
{
    private const int TEXTURE_2D_MAX_HEIGHT = 16384;

    private List<Vector3> _positionsList = new();
    private VisualEffect currentEffect;
    private Texture2D texture2d;
    private Color[] _positions;
    private int _particleAmount;

    private const string POSITIONS_TEXTURE_NAME = "Positions";
    private const string CAPACITY_PARAM_NAME = "Capacity";

    [SerializeField] private VisualEffect effectPrefab;
    [SerializeField] private GameObject effectContainer;

    private void Start()
    {
        CreateNewVisualEffect();
        ApplyPositions();
    }

    private void FixedUpdate()
    {
        ApplyPositions();
    }

    public void CreatePoint(Vector3 position)
    {
        _positionsList.Add(position);
        _particleAmount++;

        if (_particleAmount >= TEXTURE_2D_MAX_HEIGHT)
        {
            CreateNewVisualEffect();
        }
    }

    public void ClearAllPoints()
    {

    }

    private void ApplyPositions()
    {
        // create array from list
        Vector3[] pos = _positionsList.ToArray();

        // cache position for offset
        Vector3 vfxPos = currentEffect.transform.position;

        // cache transform position
        Vector3 transformPos = transform.position;

        // cache some more stuff for faster access
        int loopLength = texture2d.width * texture2d.height;
        int posListLen = pos.Length;

        for (int i = 0; i < loopLength; i++)
        {
            Color data;

            // store the transform position on the first index
            if (i == 0)
            {
                data = new Color(transformPos.x - vfxPos.x, transformPos.y - vfxPos.y, transformPos.z - vfxPos.z, 0); ;
            }
            else if (i < posListLen - 1)
            {
                data = new Color(pos[i].x - vfxPos.x, pos[i].y - vfxPos.y, pos[i].z - vfxPos.z, 1);
            }
            else
            {
                data = new Color(0, 0, 0, 0);
            }
            _positions[i] = data;
        }

        // apply to texture
        texture2d.SetPixels(_positions);
        texture2d.Apply();

        // apply to VFX
        currentEffect.SetTexture(POSITIONS_TEXTURE_NAME, texture2d);
        currentEffect.Reinit();
    }

    private void CreateNewVisualEffect()
    {
        currentEffect = Instantiate(effectPrefab, effectContainer.transform);
        currentEffect.SetUInt(CAPACITY_PARAM_NAME, (uint)TEXTURE_2D_MAX_HEIGHT);
        texture2d = new Texture2D(TEXTURE_2D_MAX_HEIGHT, 1, TextureFormat.RGBAFloat, false);
        _positions = new Color[TEXTURE_2D_MAX_HEIGHT];
        _positionsList.Clear();
        _particleAmount = 0;
    }
}
