using UnityEngine;

public class RayGun : MonoBehaviour
{
    [SerializeField] private DotRenderer dotRenderer;
    [SerializeField] private Color dotColor = Color.white;

    [SerializeField] private GameObject rayPrefab;
    [SerializeField] private float rayDistance;

    [SerializeField] private GameObject barrelLight;

    [SerializeField] private Scanner scanner;
    [SerializeField] private Painter painter;

    public bool Scanning
    {
        get => scanner.Scanning;
        set
        {
            scanner.Scanning = value;
            Painting = !Scanning && Painting;
        }
    }

    public bool Painting
    {
        get => painter.Painting;
        set
        {
            if (Scanning)
                return;

            painter.Painting = value;
        }
    }

    private void Awake()
    {
        painter.Setup(dotRenderer, rayDistance, dotColor, rayPrefab);
        scanner.Setup(dotRenderer, rayDistance, dotColor, rayPrefab);
    }

    private void Update()
    {
        // Must set barrel light active first since Painting tries to deactivate itself every frame
        barrelLight.SetActive(Painting || Scanning);
        painter.Paint();
        scanner.Scan(Time.deltaTime);
    }

    public void AdjustPaintAngle(float scrollDelta)
    {
        painter.AdjustAngle(scrollDelta);
    }
}
