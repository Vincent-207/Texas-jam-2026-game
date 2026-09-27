using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField]
    private GridTile[,] tiles;

    [SerializeField] public int gridWidth,gridHeight;
    [SerializeField]
    private GameObject tilePrefab;

    [SerializeField] private GameObject gridBGPrefab;

    private OverlayManager _overlayManager;
    private bool isReadyToRun;
    public bool IsReadyToRun => isReadyToRun;
    private List<GameObject> backgroundTiles = new();

    private AudioOneShotManager _audioOneShotManager;
    void Awake()
    {
        // InitTiles();
        _overlayManager = FindAnyObjectByType<OverlayManager>();
        _audioOneShotManager = FindAnyObjectByType<AudioOneShotManager>();
    }
    
    public void InitTiles()
    {
        
        if (tiles != null)
        {
            foreach(GridTile tile in tiles) if(tile != null) Destroy(tile.gameObject);
            
        }
        if(backgroundTiles != null) foreach(GameObject tile in backgroundTiles) Destroy(tile.gameObject);
        backgroundTiles.Clear();
        tiles = new GridTile[gridWidth, gridHeight];
        for (int row = 0; row < gridHeight; row++)
        {
            for (int column = 0; column < gridWidth; column++)
            {
                Vector3 spawnPos = new Vector3(column * transform.localScale.x, row * transform.localScale.y, 0f);
                GridTile tile = tiles[column, row] = Instantiate(tilePrefab,spawnPos, Quaternion.identity, transform).GetComponent<GridTile>();
                tile.transform.name = "Tile " + "(" + column + "," + row + ")";
                GameObject bgTile = Instantiate(gridBGPrefab, spawnPos, Quaternion.identity, transform);
                backgroundTiles.Add(bgTile);
            }
        }
        
        transform.position = new Vector3(transform.localScale.x * -gridWidth / 2, transform.localScale.y * -gridHeight / 2, 0);
        isReadyToRun = true;
    }

    public bool TryAddBlock(Vector2Int gridPos, Block block)
    {
        GridTile tile = GetTile(gridPos);
        if(tile == null) return false;
        if (tile.GetBlock() != null) return false;
        tile.SetBlock(block);
        return true;
    }

    public bool TryAddBlock(Vector2Int gridPos, GameObject block)
    {
        GridTile tile = GetTile(gridPos);
        if(tile == null) return false;
        if (tile.GetBlock() != null) return false;
        Block instantiatedBlock = Instantiate(block).GetComponent<Block>();
        tile.SetBlock(instantiatedBlock);
        return true;
    }

    public GridTile GetTile(Vector2 worldPos)
    {
        return GetTile(WorldToGridPos(worldPos));
    }
    public GridTile GetTile(Vector2Int pos)
    {
        if(!IsGridPosWithinBounds(pos)) return null;
        return tiles[pos.x, pos.y];
    }

    public bool IsTilePlaceable(Vector2 worldPos)
    {
        Vector2Int gridPos = WorldToGridPos(worldPos);
        if (IsGridPosWithinBounds(gridPos))
        {
            // Debug.Log("Is in grid");
            if (GetTile(gridPos).GetBlock() == null) return true;
            // Debug.Log("No null block there :(");
        }

        return false;
    }

    public bool IsPointOverGrid(Vector2 worldPos)
    {
        Vector2Int gridPos = WorldToGridPos(worldPos);
        return IsGridPosWithinBounds(gridPos);
        return true;
    }

    public Vector2Int WorldToGridPos(Vector2 worldPos)
    {
        // Debug.Log("world pos: " + worldPos);
        Vector2 TwoDPos = (Vector2)transform.position;
        Vector2 relativePos = worldPos - TwoDPos;
        relativePos.x /= transform.localScale.x;
        relativePos.y /= transform.localScale.y;
        // Debug.Log("REL: " + relativePos);
        
        // getTile(Vector2Int.RoundToInt(relativePos)).DebugSetColor(Color.rebeccaPurple);
        return Vector2Int.RoundToInt(relativePos);
    }

    bool IsGridPosWithinBounds(Vector2Int gridPos)
    {
        if (gridPos.x < 0 || gridPos.x > gridWidth || gridPos.y < 0 || gridPos.y > gridHeight) return false;
        return true;
    }
    public void UpdateGrid()
    {
        isReadyToRun = false;
        Block[,] blocks = new Block[gridWidth, gridHeight];
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                if(tiles[x,y] == null) continue;
                blocks[x, y] = tiles[x, y].GetBlock();
                // Debug.Log("Found block: " );
            }
        }
        
        //Update block values. 
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                if (blocks[x, y] == null) continue;
                blocks[x,y].SetValues(blocks, new Vector2Int(x, y));
            }
        }
        
        
        StartCoroutine(ProcessBlocks(blocks));
        
        // CreateAndUseUpdatedGrid(blocks);
    }

    IEnumerator ProcessBlocks(Block[,] blocks)
    {
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                if(blocks[x,y] == null) continue;
                float blockTime = blocks[x,y].Process();
                Debug.Log("Block Time: " + blockTime);
                yield return new WaitForSeconds(blockTime);
                
            }
        }
        
        StartCoroutine(WaitForAnimFinish(blocks));
            
    }

    void CreateAndUseUpdatedGrid(Block[,] blocks)
    {
        //Create grid with updated blocks
        Block[,] newGrid = new Block[gridWidth, gridHeight];
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                if (blocks[x, y] == null)
                {
                    continue;
                }
                Vector2Int blockPos = blocks[x, y].nextPos;
                if (!IsGridPosWithinBounds(blockPos))
                {
                    Debug.LogWarning("Block is trying to move out of bounds. I'm destroying it.");
                    Block block = blocks[x, y];
                    // Block block2 = newGrid[blockPos.x, blockPos.y];
                    continue;
                }
                if(newGrid[blockPos.x, blockPos.y] != null) ShowDestruction();
                newGrid[blockPos.x, blockPos.y] = blocks[x, y];
                
            }
        }
        
        //apply new grid to tiles.
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                tiles[x, y].SetBlock(newGrid[x, y]);
                
            }
        }
        
    }

    void ShowDestruction()
    {
        Debug.Log("Block is destroying/overwriting.");
        _audioOneShotManager.PlayDestroySound();
    }
    
    
    IEnumerator  WaitForAnimFinish(Block[,] blocks)
    {
        // yield return new WaitForSeconds(2f);
        _overlayManager.ClearOverlays();
        CreateAndUseUpdatedGrid(blocks);
        isReadyToRun = true;
        yield return null;
    }
    
    

}

