using System;
using UnityEngine;
using UnityEngine.UI;

public class RunButton : MonoBehaviour
{
    private Button button;
    GridController gridController;
    BlockPlacer blockPlacer;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        gridController = FindAnyObjectByType<GridController>();
        blockPlacer = FindAnyObjectByType<BlockPlacer>();
    }

    void OnClick()
    {
        if (gridController.IsReadyToRun)
        {
            gridController.UpdateGrid();
            blockPlacer.DisablePlacing();
        }
    }
}
