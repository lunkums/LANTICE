using UnityEngine;

public class Init : MonoBehaviour
{
    [SerializeField] private int targetFrameRate;
    [SerializeField] public AudioSource backgroundPianoAudioSource;

    public static Init Instance { get; private set; }

    private void Awake()
    {
        Instance = Instance != null ? Instance : this;

        backgroundPianoAudioSource.Play();

        Application.targetFrameRate = targetFrameRate;
    }
}