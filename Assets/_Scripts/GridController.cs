using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField]
    private GridTile[,] tiles;

    [SerializeField] private int gridWidth,gridHeight;
    [SerializeField]
    private GameObject tilePrefab;


    void Awake()
    {
        InitTiles();
    }
    
    void InitTiles()
    {
        tiles = new GridTile[gridWidth, gridHeight];
        for (int row = 0; row < gridHeight; row++)
        {
            for (int column = 0; column < gridWidth; column++)
            {
                GridTile tile = tiles[column, row] = Instantiate(tilePrefab, new Vector3(column * transform.localScale.x, row * transform.localScale.y, 0f), Quaternion.identity, transform).GetComponent<GridTile>();
                tile.transform.name = "Tile " + "(" + column + "," + row + ")";
                
            }
        }
        
        transform.position = new Vector3(transform.localScale.x * -gridWidth / 2, transform.localScale.y * -gridHeight / 2, 0);
    }

    public bool TryAddBlock(Vector2Int gridPos, Block block)
    {
        GridTile tile = GetTile(gridPos);
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
        
        //Process blocks
        
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                if(blocks[x,y] == null) continue;
                blocks[x,y].Process();
            }
        }
        
        
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
                newGrid[blockPos.x, blockPos.y] = blocks[x, y];

            }
        }
        
        // Debug.Log("New grid!");
        foreach (Block block in blocks)
        {
            if (block == null)
            {
                continue;
            }
            // else Debug.Log("Block: " + block.name);
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
    

}
