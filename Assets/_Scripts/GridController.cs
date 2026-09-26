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

    public GridTile getTile(Vector2Int pos)
    {
        return tiles[pos.x, pos.y];
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
        // Debug.Log("POST PROCESS!");
        foreach (Block block in blocks)
        {
            if (block == null)
            {
                continue;
            }
            // else Debug.Log("Block: " + block.name);
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
                Vector2Int blockPos = blocks[x, y].pos;
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
