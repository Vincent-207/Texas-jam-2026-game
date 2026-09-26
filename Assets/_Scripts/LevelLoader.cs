using System;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    GridController gridController;
    [SerializeField]
    LevelDataSO levelData;
    private void Awake()
    {
        gridController = transform.parent.GetComponent<GridController>();
    }

    void Start()
    {
        TileInfo[] tileInfos = levelData.Tiles;
        foreach (TileInfo tileInfo in tileInfos)
        {
            Block block = Instantiate(tileInfo.BlockPrefab).GetComponent<Block>();
            gridController.TryAddBlock(tileInfo.Position, block);
        }
    }
}
