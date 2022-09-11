using UnityEngine;

public class Invisibleinator : MonoBehaviour
{
    private const string ENVIRONMENT_LAYER = "Environment";

    private void Awake()
    {
        Invisibleify();
        SetLayer(gameObject);
    }

    private void Invisibleify()
    {
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>(true);

        foreach (MeshRenderer renderer in renderers)
        {
            SetLayer(renderer.gameObject);
            renderer.enabled = false;
        }
    }

    private void SetLayer(GameObject gameObject)
    {
        gameObject.layer = LayerMask.NameToLayer(ENVIRONMENT_LAYER);
    }
}
