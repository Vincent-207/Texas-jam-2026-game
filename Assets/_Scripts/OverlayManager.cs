using System.Collections.Generic;
using UnityEngine;

public class OverlayManager : MonoBehaviour
{
    TileAnimator tileAnimator;
    [SerializeField]
    private GameObject arrowOverlayPrefab;
    List<ArrowOverlay> overlays = new();
    void Awake()
    {
        tileAnimator = FindAnyObjectByType<TileAnimator>();
    }
    public void AddOverlay(Vector2Int direction, GridTile tile)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        GameObject arrowOverlay = Instantiate(arrowOverlayPrefab, tile.transform.position, 
            Quaternion.Euler(0f, 0f, angle + 90f), tile.transform);
        overlays.Add(arrowOverlay.GetComponent<ArrowOverlay>());
    }

    public void ClearOverlays()
    {
        foreach (ArrowOverlay arrowOverlay in overlays)
        {
            if(arrowOverlay != null) Destroy(arrowOverlay.gameObject);
        }
        overlays.Clear();
    }
}
