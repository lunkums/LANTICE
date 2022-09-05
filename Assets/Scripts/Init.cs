using UnityEngine;

public class Init : MonoBehaviour
{
    [SerializeField] private int targetFrameRate;

    private void Awake()
    {
        Application.targetFrameRate = targetFrameRate;
    }
}
