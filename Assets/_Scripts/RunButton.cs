using System;
using UnityEngine;
using UnityEngine.UI;

public class RunButton : MonoBehaviour
{
    private Button button;
    GridController gridController;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        gridController = FindAnyObjectByType<GridController>();
    }

    void OnClick()
    {
        if (gridController.IsReadyToRun)
        {
            gridController.UpdateGrid();
        }
    }
}
