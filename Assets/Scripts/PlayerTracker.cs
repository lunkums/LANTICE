using UnityEngine;

public class PlayerTracker : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private PointRenderer pointRenderer;

    private void Update()
    {
        pointRenderer.SetReferencePosition(player.position);
    }
}
