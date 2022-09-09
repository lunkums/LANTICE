using UnityEngine;
using UnityEngine.VFX;

public class PointRenderer : MonoBehaviour
{
    // Value was chosen to match Unity examples
    private const int CAPACITY = 65536;

    [SerializeField] private Transform effectsContainer;
    [SerializeField] private VisualEffect visualEffectPrefab;

    [SerializeField] private string spawnEventName;
    [SerializeField] private string positionPropertyName;

    private VisualEffect currentEffect;
    private int particleCount;

    private void Awake()
    {
        SetNewEffect();
    }

    public void CreatePoint(Vector3 position)
    {
        if (particleCount >= CAPACITY)
        {
            SetNewEffect();
        }

        currentEffect.SetVector3(positionPropertyName, position);
        currentEffect.SendEvent(spawnEventName);
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
        particleCount = 0;
    }
}
