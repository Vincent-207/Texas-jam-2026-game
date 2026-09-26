using System;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    GridController gridController;
    [SerializeField]
    LevelDataSO levelData;
    [SerializeField] private Vector3 gridPos;
    [SerializeField]
    private int gridWidth, gridHeight;
    private void Awake()
    {
        gridController = transform.parent.GetComponent<GridController>();
    }

    void Start()
    {
        gridController.gridWidth = gridWidth;
        gridController.gridHeight = gridHeight;
        gridController.InitTiles();
        
        TileInfo[] tileInfos = levelData.Tiles;
        foreach (TileInfo tileInfo in tileInfos)
        {
            Block block = Instantiate(tileInfo.BlockPrefab).GetComponent<Block>();
            gridController.TryAddBlock(tileInfo.Position, block);
        }
        
        gridController.transform.position = gridPos;
    }
}
