using System.IO;
using UnityEngine;

public class PullBlock : Block
{
    public Vector2Int direction = Vector2Int.right;
    public override void SetValues(Block[,] newBlocks, Vector2Int pos)
    {
        blocks = newBlocks;
        this.pos = pos;
    }
    
    public override void Process()
    {
        Debug.Log("Processing Kinetic Block!");
       
        Vector2Int newCoords = pos + direction;

        if (!AreCordsInGrid(newCoords)) return;
        
        
        Block nextTile = blocks[newCoords.x, newCoords.y];
        if(nextTile == null || (nextTile != null && nextTile.CanPush(direction)))
        {
            if(nextTile != null) nextTile.Push(direction);
            pos += direction;
            // Debug.Log("Updated pos: " + pos);
            // TryToPull();
        }
    }

    void TryToPull()
    {
        //Get closest block left.
        Block closestBlockBehind = null;
        Vector2Int coordsBehind = pos - direction;
        while (closestBlockBehind == null)
        {
            if (blocks[coordsBehind.x, coordsBehind.y] == null)
            {
                coordsBehind -= direction;
            }
            else
            {
                closestBlockBehind =  blocks[coordsBehind.x, coordsBehind.y];
            }
        }

        if (closestBlockBehind == null)
        {
            Debug.Log("Nothing to pull!");
        }

        if (closestBlockBehind.CanPush(direction))
        {
            closestBlockBehind.Push(direction);
        }
        else
        {
            return;
        }
    }
}
