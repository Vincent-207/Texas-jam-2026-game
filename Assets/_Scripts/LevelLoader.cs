using System;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    GridController gridController;
    [SerializeField]
    LevelDataSO levelData;
    BlockPlacer blockPlacer;
    private void Awake()
    {
        blockPlacer = FindAnyObjectByType<BlockPlacer>();
        gridController = transform.parent.GetComponent<GridController>();
        
    }

    void Start()
    {
        gridController.gridWidth = levelData.GridWidth;
        gridController.gridHeight = levelData.GridHeight;
        
        gridController.InitTiles();
        TileInfo[] tileInfos = levelData.Tiles;
        foreach (TileInfo tileInfo in tileInfos)
        {
            Block block = Instantiate(tileInfo.BlockPrefab).GetComponent<Block>();
            IRotateable rotateable = block.GetComponent<IRotateable>();
            if(tileInfo.Direction != Vector2Int.zero && rotateable != null) rotateable.SetRotate(tileInfo.Direction); 
            gridController.TryAddBlock(tileInfo.Position, block);
        }
        
        if(levelData.blockPlaceDirection != Vector2Int.zero) blockPlacer.blockPlaceDir = levelData.blockPlaceDirection;
        
    }
}
