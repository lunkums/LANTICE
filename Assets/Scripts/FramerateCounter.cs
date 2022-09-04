using UnityEngine;

public class FramerateCounter : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text text;

    private float pollingTime;
    private float time;
    private int frameCount;

    private void Awake()
    {
        pollingTime = 1;
    }

    void Update()
    {
        time += Time.deltaTime;
        frameCount++;

        if (time > pollingTime)
        {
            text.text = Mathf.RoundToInt(frameCount / time).ToString() + " FPS";
            time -= pollingTime;
            frameCount = 0;
        }
    }
}