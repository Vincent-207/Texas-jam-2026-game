using System;
using UnityEngine;

public class GoalChecker : MonoBehaviour
{
    [SerializeField] private Vector2Int goalPos;
    GridController gridController;
    [SerializeField]
    private CanvasGroup winPanel;
    [SerializeField]
    private Transform goalOverlay;
    private void Awake()
    {
        gridController = FindAnyObjectByType<GridController>();
    }

    void Update()
    {
        GridTile tile = gridController.GetTile(goalPos);
        if (tile == null)
        {
            return;
        } 
        goalOverlay.position = tile.transform.position;
        if (tile.GetBlock() == null) return;
        if (gridController.GetTile(goalPos).GetBlock().IsObjectiveBlock())
        {
            winPanel.alpha = 1;
            winPanel.blocksRaycasts = true;
            winPanel.interactable = true;

        }
    }
}
